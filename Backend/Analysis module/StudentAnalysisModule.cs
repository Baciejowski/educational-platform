using System;
using System.Collections.Generic;
using Backend.Analysis_module.Models;

namespace Backend.Analysis_module
{
    public class StudentAnalysisModule
    {
        public double DifficultyLevel { get; set; }
        readonly Random _random = new Random();
        private List<QuestionAnalysisModel> _questionAnalysisModels = new List<QuestionAnalysisModel>();

        public StudentAnalysisModule()
        {
        }

        public void CalculateLvl()
        {
            DifficultyLevel = _random.NextDouble() * 5;
        }

        public void AddQuestionToAnalysis(AnsweredQuestionModel question)
        {
            QuestionAnalysisModel questionAnalysisModel = new QuestionAnalysisModel
            {
                QuestionType = question.QuestionType
            };
            _questionAnalysisModels.Add(questionAnalysisModel);
            CalculateLvl();
        }
    }
}