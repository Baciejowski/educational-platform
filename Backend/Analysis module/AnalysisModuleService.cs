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
        private List<StudentSessionModule> _studentSessionModules = new List<StudentSessionModule>();

        public AnalysisModuleService(DataContext context)
        {
            Context = context;
        }

        public StartGameResponse StartNewSession(StartGameRequest request)
        {
            var id = GenerateGameId();
            StudentSessionModule studentSessionModule;
            if (request.Email == "test" && request.Code == "test")
            {
                var itemToRemove = _studentSessionModules.FirstOrDefault(x => x.Email == "test" && x.Code == "test");
                if (itemToRemove != null)
                    _studentSessionModules.Remove(itemToRemove);

                studentSessionModule = new StudentSessionModule(request.Email, 1, request.Code, id);
            }
            else
            {
                var userSession = Context.Sessions
                    .Include(s => s.Student)
                    .Include(s => s.Scenario)
                    .ThenInclude(scenario => scenario.Questions)
                    .Include(s=>s.Topic)
                    .FirstOrDefault(x => x.Code == request.Code && x.Student.Email == request.Email);

                var error = CheckUserConditions(userSession);
                if (error != null)
                    return error;

                // userSession.Attempts++;

                studentSessionModule =
                    new StudentSessionModule(request.Email, userSession.Student.StudentID, request.Code, id,
                        userSession);
            }

            _studentSessionModules.Add(studentSessionModule);
            return new StartGameResponse
            {
                SessionCode = id,
                QuestionsNumber = { studentSessionModule.GetQuestionsAmount() },
                Error = false,
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

            user.SaveAnswerResponse(StudentResponseAdapter(request));
            return new Empty();
        }

        public Empty EndGame(EndGameRequest request)
        {
            return new Empty();
        }

        private StudentSessionModule GetUser(string sessionCode)
        {
            var result =
                _studentSessionModules.FirstOrDefault(x => x.SessionId == sessionCode);
            return result;
        }

        private static AnsweredQuestionModel StudentResponseAdapter(StudentAnswerRequest request)
        {
            return new AnsweredQuestionModel
            {
                QuestionImportanceType = (QuestionImportanceType)((int)request.QuestionType),
                AnswersId = request.AnswersID,
                TimeToAnswer = request.TimeToAnswer
            };
        }

        private IEnumerable<QuestionResponse.Types.Answer> grpcQuestionResponseAnswersAdapter(IReadOnlyList<Answer> answers)
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

            // if(userSession.Attempts>0)
            // {
            //     return new StartGameResponse
            //     {
            //         Error = true,
            //         ErrorMsg = "Sorry! You have reached the limit of attempts."
            //     };
            // }

            return null;
        }
    }
}