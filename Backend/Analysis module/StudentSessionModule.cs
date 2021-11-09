using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.Models;
using Backend.Models;

namespace Backend.Analysis_module
{
    public enum QuestionType
    {
        Required = 0,
        Important = 1,
        Basic = 2
    }

    public class StudentSessionModule
    {
        public string StudentEmail { get; set; }
        public int StudentId { get; set; }
        public int SessionId { get; set; }
        public string Code { get; set; }

        private Question[] _readyQuestions;
        private List<Question>[] _availableQuestions;
        private StudentAnalysisModule _studentAnalysisModule = new StudentAnalysisModule();
        private readonly Random _random = new Random();

        public StudentSessionModule(string studentEmail, int studentId, string code, int sessionId)
        {
            StudentEmail = studentEmail;
            StudentId = studentId;
            Code = code;
            SessionId = sessionId;

            _availableQuestions = GetQuestionsFromScenario();
            _readyQuestions = SetInitialQuestions();
        }

        private List<Question>[] GetQuestionsFromScenario()
        {
            var result = new List<Question>[3];
            foreach (var questions in result)
            {
                questions.Add(new Question());
            }

            return result;
        }

        private Question[] SetInitialQuestions()
        {
            var result = new Question[3];
            for (var i = 0; i < 3; i++)
            {
                var index = _random.Next(_availableQuestions[i].Count);
                result[i] = _availableQuestions[i].ElementAt(index);
            }

            return result;
        }

        public Question GetQuestion(QuestionType questionType)
        {
            return _readyQuestions[(int)questionType];
        }

        public AnsweredQuestionModel GetAnswerResponse(AnsweredQuestionModel answeredQuestion)
        {
            CalculateReward(answeredQuestion);
            _studentAnalysisModule.AddQuestionToAnalysis(answeredQuestion);
            FindNextQuestion(answeredQuestion.QuestionType);
            return answeredQuestion;
        }

        private void CalculateReward(AnsweredQuestionModel answeredQuestion)
        {
            throw new NotImplementedException();
        }

        private void FindNextQuestion(QuestionType questionType)
        {
            RemoveQuestion(questionType);
            throw new NotImplementedException();
        }

        private void RemoveQuestion(QuestionType questionType)
        {
            throw new NotImplementedException();
        }
    }
}