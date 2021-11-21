using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.Models;
using Backend.Models;
using Gameplay;
using Microsoft.EntityFrameworkCore;

namespace Backend.Analysis_module
{
    public class AnalysisModuleService : IAnalysisModuleService
    {
        protected readonly DataContext Context;
        private readonly IStudentSessionFactory _studentSessionFactory;
        private List<StudentSessionModule> _studentSessionModules = new List<StudentSessionModule>();

        public AnalysisModuleService(DataContext context, IStudentSessionFactory studentSessionFactory)
        {
            Context = context;
            _studentSessionFactory = studentSessionFactory;

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(15);

            var timer = new System.Threading.Timer((e) => FindAbandonedScenarios(), null, startTimeSpan,
                periodTimeSpan);
        }

        public StartGameResponse StartNewSession(StartGameRequest request)
        {
            var id = GenerateGameId();
            StudentSessionModule studentSessionModule;
            if (request.Email.ToLower() == "test" && request.Code.ToLower() == "test")
            {
                RemoveUser("test",  "test");

                studentSessionModule = _studentSessionFactory.Create(request.Email, 1, request.Code, id);
            }
            else
            {
                var userSession = Context.Sessions
                    .Include(s => s.Student)
                    .Include(s => s.Scenario)
                    .ThenInclude(scenario => scenario.Questions)
                    .Include(s => s.Topic)
                    .FirstOrDefault(x =>
                        x.Code == request.Code.ToLower() && x.Student.Email == request.Email.ToLower());

                var error = CheckUserConditions(userSession);
                if (error != null)
                    return error;

                userSession.Attempts++;

                studentSessionModule =
                    _studentSessionFactory.Create(request.Email, userSession.Student.StudentID, request.Code, id,
                        userSession);
            }

            _studentSessionModules.Add(studentSessionModule);
            return new StartGameResponse
            {
                SessionCode = id,
                QuestionsNumber = { studentSessionModule.GetQuestionsAmount() },
                Error = false,
                MazeSetting = new StartGameResponse.Types.MazeSetting(),
                StudentData = new StartGameResponse.Types.StudentData
                {
                    Experience = 0,
                    Money = 0
                } // TO DO  - pobieranie statow ucznia
            };
        }

        public QuestionResponse PrepareNextQuestion(QuestionRequest request)
        {
            var user = GetUser(request.SessionCode);

            var question = user.GetQuestion((QuestionImportanceType)request.QuestionType);

            var answers = grpcQuestionResponseAnswersAdapter((List<Answer>)question.ABCDAnswers);
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

            }

            return new Empty();
        }

        private void FindAbandonedScenarios()
        {
            foreach (var studentSessionModule in _studentSessionModules.Where(studentSessionModule => studentSessionModule.IsAbandoned()))
            {
                RemoveUser(studentSessionModule);
            }
        }

        private void RemoveUser(StudentSessionModule studentSessionModule)
        {
            _studentSessionModules.Remove(studentSessionModule);
        }

        private void RemoveUser(string email, string code)
        {

            var itemToRemove = _studentSessionModules.FirstOrDefault(x =>
                x.StudentSessionData.Email == email && x.StudentSessionData.Code == code);
            if (itemToRemove != null)
                RemoveUser(itemToRemove);
        }

        private StudentSessionModule GetUser(string sessionCode)
        {
            var result =
                _studentSessionModules.FirstOrDefault(x => x.StudentSessionData.SessionId == sessionCode);
            return result;
        }


        private IEnumerable<QuestionResponse.Types.Answer> grpcQuestionResponseAnswersAdapter(
            IReadOnlyList<Answer> answers)
        {
            var result = new QuestionResponse.Types.Answer[answers.Count];
            for (var i = 0; i < answers.Count; i++)
            {
                result[i] = new QuestionResponse.Types.Answer
                {
                    AnswersID = answers[i].AnswerID,
                    Content = answers[i].Content,
                    Correct = answers[i].Correct
                };
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