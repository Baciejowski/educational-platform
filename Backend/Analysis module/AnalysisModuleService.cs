using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.Models;
using Backend.Analysis_module.SessionModule;
using Backend.Models;
using Gameplay;
using Microsoft.EntityFrameworkCore;

namespace Backend.Analysis_module
{
    public class AnalysisModuleService : IAnalysisModuleService
    {
        private readonly ISessionFactory _sessionFactory;
        private List<SessionModuleService> _sessionModules = new List<SessionModuleService>();

        public AnalysisModuleService(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(15);

            var timer = new System.Threading.Timer((e) => FindAbandonedScenarios(), null, startTimeSpan,
                periodTimeSpan);
        }

        public StartGameResponse StartNewSession(StartGameRequest request, DataContext Context)
        {
            var id = GenerateGameId();
            SessionModuleService sessionModule;
            var studentData = new StartGameResponse.Types.StudentData
            {
                Experience = 0,
                Money = 0
            };
            if (request.Email.ToLower() == "test" && request.Code.ToLower() == "test")
            {
                RemoveUser("test", "test");

                sessionModule = _sessionFactory.Create(request.Email, 1, request.Code, id, Context);
            }
            else
            {
                Session userSession;
                try
                {
                    var sessions = Context.Sessions
                        .Include(s => s.Student)
                        .Include(s => s.Scenario)
                        // .Include(s => s.Scenario)
                        // .ThenInclude(scenario => scenario.Topic)
                        .ThenInclude(scenario => scenario.Questions)
                        .ThenInclude(questions => questions.ABCDAnswers).ToList();

                    userSession = sessions.First(x => x.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase) &&
                                                      x.Student.Email.Equals(request.Email,
                                                          StringComparison.OrdinalIgnoreCase));
                    var sessionRecords = Context.SessionRecords
                        .Include(x => x.GameplayData)
                        .Include(x => x.Session)
                        .ThenInclude(x => x.Student);
                    var lastGameplay = sessionRecords
                        .Where(x => x.Session.Student.StudentID == userSession.Student.StudentID)
                        .OrderBy(x => x.SessionRecordID)
                        .LastOrDefault()
                        ?.GameplayData;
                    if (lastGameplay != null) studentData.Experience = lastGameplay.Experience;
                }
                catch
                {
                    return new StartGameResponse
                    {
                        Error = true,
                        ErrorMsg = "Wrong credentials."
                    };
                }

                var error = CheckUserConditions(userSession);
                if (error != null)
                    return error;

                //TO DO odkomentować przed testami na uzytkownikach
                // userSession.Attempts++;
                // Context.SaveChanges();

                sessionModule =
                    _sessionFactory.Create(request.Email, userSession.Student.StudentID, request.Code, id,
                        userSession, Context);
            }

            _sessionModules.Add(sessionModule);
            return new StartGameResponse
            {
                SessionCode = id,
                QuestionsNumber = { sessionModule.GetQuestionsAmount() },
                Error = false,
                MazeSetting = new StartGameResponse.Types.MazeSetting(),
                StudentData = studentData
            };
        }

        public QuestionResponse PrepareNextQuestion(QuestionRequest request)
        {
            var user = GetUser(request.SessionCode);

            var question = user.GetQuestion((QuestionImportanceType)request.QuestionType);

            var answers = grpcQuestionResponseAnswersAdapter(question.ABCDAnswers);
            var result = new QuestionResponse()
            {
                SessionCode = request.SessionCode,
                Content = question.Content,
                Answers = { answers },
                QuestionReward = user.CalculateReward()
            };
            return result;
        }

        public Empty UpdateStudentsAnswers(StudentAnswerRequest request)
        {
            var user = GetUser(request.SessionCode);

            user.SaveAnswerResponse(request);
            return new Empty();
        }

        public Empty EndGame(EndGameRequest request)
        {
            var user = GetUser(request.SessionCode);

            if (user.EndGame(request))
            {
                RemoveUser(user);
            }

            return new Empty();
        }

        private void FindAbandonedScenarios()
        {
            foreach (var SessionModule in _sessionModules.Where(SessionModule => SessionModule.IsAbandoned()))
            {
                RemoveUser(SessionModule);
            }
        }

        private void RemoveUser(SessionModuleService sessionModule)
        {
            _sessionModules.Remove(sessionModule);
        }

        private void RemoveUser(string email, string code)
        {
            var itemToRemove = _sessionModules.FirstOrDefault(x =>
                x.StudentData.Email == email && x.StudentData.Code == code);
            if (itemToRemove != null)
                RemoveUser(itemToRemove);
        }

        private SessionModuleService GetUser(string sessionCode)
        {
            var result =
                _sessionModules.FirstOrDefault(x => x.StudentData.SessionId == sessionCode);
            return result;
        }


        private IEnumerable<QuestionResponse.Types.Answer> grpcQuestionResponseAnswersAdapter(
            ICollection<Answer> answers)
        {
            var result = new QuestionResponse.Types.Answer[answers.Count];
            var i = 0;
            foreach (var answer in answers)
            {
                result[i] = new QuestionResponse.Types.Answer
                {
                    AnswersID = answer.AnswerID,
                    Content = answer.Content,
                    Correct = answer.Correct
                };
                i++;
            }

            return result;
        }

        private static bool BetweenDates(DateTime input, DateTime date1, DateTime date2)
        {
            return (input > date1 && input < date2);
        }

        private static string GenerateGameId()
        {
            return Guid.NewGuid().ToString();
        }

        private static StartGameResponse CheckUserConditions(Session userSession)
        {
            if (userSession == null)
            {
                return new StartGameResponse
                {
                    Error = true,
                    ErrorMsg = "Wrong credentials."
                };
            }

            if (!BetweenDates(DateTime.Now, userSession.StartGame, userSession.EndGame))
            {
                return new StartGameResponse
                {
                    Error = true,
                    ErrorMsg = "Sorry! Your game expired."
                };
            }

            if (userSession.Attempts > 0)
            {
                return new StartGameResponse
                {
                    Error = true,
                    ErrorMsg = "Sorry! You have reached the limit of attempts."
                };
            }

            return null;
        }
    }
}