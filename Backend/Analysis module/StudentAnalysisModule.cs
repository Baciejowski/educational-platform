using System;
using System.Collections.Generic;
using Backend.Analysis_module.Models;

namespace Backend.Analysis_module
{
    public class StudentAnalysisModule
    {
        public double DifficultyLevel { get; set; }
        private readonly Random _random = new Random();
        private List<AnsweredQuestionModel> _answeredQuestionsModels = new List<AnsweredQuestionModel>();

        public void CalculateLvl()
        {
            DifficultyLevel = _random.NextDouble() * 5;
        }

        public void AddQuestionToAnalysis(AnsweredQuestionModel question)
        {
            _answeredQuestionsModels.Add(question);
            CalculateLvl();
        }
    }
}