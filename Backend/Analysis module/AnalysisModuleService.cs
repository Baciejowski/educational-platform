using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.Models;
using Backend.Models;
using Gameplay;

namespace Backend.Analysis_module
{
    public class AnalysisModuleService : IAnalysisModuleService
    {
        private List<StudentSessionModule> _studentSessionModules = new List<StudentSessionModule>();

        public StartGameResponse StartNewSession(StartGameRequest request)
        {
            if (request.Email == "test" && request.Code == "test")
            {
                var itemToRemove = _studentSessionModules.FirstOrDefault(x => x.Email == "test" && x.Code == "test");
                if (itemToRemove != null)
                    _studentSessionModules.Remove(itemToRemove);

                var guid = Guid.NewGuid();
                var id = guid.ToString();
                var testUser = new StudentSessionModule(request.Email, 1, request.Code, id);
                _studentSessionModules.Add(testUser);
                return new StartGameResponse
                {
                    SessionCode = id,
                    Error = false,
                    StudentData = new StartGameResponse.Types.StudentData
                    {
                        Experience = 0,
                        Money = 0
                    }
                };
            }

            return new StartGameResponse
            {
                Error = true,
                ErrorMsg = "Not implemented"
            };
        }

        public QuestionResponse PrepareNextQuestion(QuestionRequest request)
        {
            var user = GetUser(request.SessionCode);

            var question = user.GetQuestion((QuestionImportanceType) request.QuestionType);
            var answers = grpcQuestionResponseAnswersAdapter((List<Answer>)question.ABCDAnswers);
            var result =  new QuestionResponse()
            {
                SessionCode = request.SessionCode,
                Content = question.Content, 
                QuestionReward = user.CalculateReward(),
            };
            result.Answers.Add(answers);
            return result;
        }

        private StudentSessionModule GetUser(string sessionCode)
        {
            var result = 
            _studentSessionModules.FirstOrDefault(x => x.SessionId == sessionCode);

            if (result == null) throw new NullReferenceException();
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

        private static AnsweredQuestionModel StudentResponseAdapter(StudentAnswerRequest request)
        {
            return new AnsweredQuestionModel
            {

                QuestionImportanceType= (QuestionImportanceType)((int)request.QuestionType),
                AnswersId = request.AnswersID ,
                TimeToAnswer = request.TimeToAnswer 
        };
        }

        private IEnumerable<QuestionResponse.Types.Answer> grpcQuestionResponseAnswersAdapter(List<Answer> answers)
        {
            var result = new QuestionResponse.Types.Answer[answers.Count];
            for (var i = 0; i < answers.Count; i++)
            {
                result[i] = new QuestionResponse.Types.Answer
                {
                    AnswersID = (uint)answers[i].AnswerID,
                    Content = answers[i].Content,
                    Correct = answers[i].Correct
                };
            }

            return result;
        }
    }
}