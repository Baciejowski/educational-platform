using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.Models;

namespace Backend.Analysis_module
{
    public class StudentAnalysisModule
    {
        public double DifficultyLevel { get; set; }
        private readonly Random _random = new Random();
        private List<AnsweredQuestionModel> _answeredQuestionsModels = new List<AnsweredQuestionModel>();
        private readonly bool _controlUser = false;
        private readonly int _testLimit;
        private readonly float _prevRank = 0;

        public StudentAnalysisModule(int testLimit)
        {
            _testLimit = testLimit;
        }

        public StudentAnalysisModule(bool randomTest, int testLimit) : this(testLimit)
        {
            _controlUser = randomTest;
            //TO DO: get prev rank
        }

        public float GetProbability()
        {
            return (float)(DifficultyLevel - Math.Floor(DifficultyLevel));
        }

        public int GetNextQuestionDifficulty()
        {
            var rand = (float)_random.NextDouble();
            var probability = GetProbability();
            return rand < probability
                ? (int)Math.Floor(DifficultyLevel)
                : (int)Math.Ceiling(DifficultyLevel);
        }

        public void CalculateLvl()
        {
            if (_controlUser || _answeredQuestionsModels.Count < _testLimit)
                DifficultyLevel = -1;
            else
            {
                var value = _prevRank;
                var divider = 1;
                for (var i = 0; i < _answeredQuestionsModels.Count; i++)
                {
                    value += (i + 1) * _answeredQuestionsModels[i].Correctness *
                             _answeredQuestionsModels[i].Question.Difficulty;
                    divider += (i + 1);
                }

                DifficultyLevel = Math.Max(value / (float)divider, 0);
            }
        }

        public void AddQuestionToAnalysis(AnsweredQuestionModel question)
        {
            question.Correctness = CalculateCorrectness(question);
            _answeredQuestionsModels.Add(question);
            CalculateLvl();
        }

        private static float CalculateCorrectness(AnsweredQuestionModel question)
        {
            var answers = question.Question.ABCDAnswers.Where(x => question.AnswersId.Contains(x.AnswerID)).ToList();
            var correctCount = question.Question.ABCDAnswers.Count(x => x.Correct);
            return answers.Where(answer => !answer.Correct)
                .Aggregate(1.0f, (current, answer) => current - 1f / correctCount);
        }
    }
}