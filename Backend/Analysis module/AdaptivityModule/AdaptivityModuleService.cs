using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;

namespace Backend.Analysis_module.AdaptivityModule
{
    public class AdaptivityModuleService
    {
        public double DifficultyLevel { get; set; }
        public bool AiCategorization { get; set; }
        public bool RandomTest { get; set; }

        private readonly Random _random = new Random();
        private List<AnsweredQuestion> _answeredQuestionsModels = new List<AnsweredQuestion>();
        private readonly int _testLimit;
        private readonly float _prevRank = 0;

        public AdaptivityModuleService(int testLimit)
        {
            _testLimit = testLimit;
            AiCategorization = false;
            RandomTest = false;
        }

        public AdaptivityModuleService(bool randomTest, int testLimit) : this(testLimit)
        {
            RandomTest = randomTest;
            //TO DO: get prev rank
        }

        public AdaptivityModuleService(bool randomTest, bool aiCategorization, int testLimit) : this(testLimit)
        {
            RandomTest = randomTest;
            AiCategorization = aiCategorization;
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
                ? (int)Math.Max(Math.Floor(DifficultyLevel), 1)
                : (int)Math.Min(Math.Ceiling(DifficultyLevel), 5);
        }

        public void CalculateLvl()
        {
            if (RandomTest || _answeredQuestionsModels.Count < _testLimit)
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

                DifficultyLevel = Math.Max(value / (float)divider, 1);
            }
        }

        public void AddQuestionToAnalysis(AnsweredQuestion question)
        {
            question.Correctness = CalculateCorrectness(question);
            _answeredQuestionsModels.Add(question);
            CalculateLvl();
        }

        private static float CalculateCorrectness(AnsweredQuestion question)
        {
            var correctCount = question.Question.ABCDAnswers.Count(x => x.Correct);
            var answers = question.Question.ABCDAnswers.Where(x => question.AnsweredAnswers.Contains(x.AnswerID))
                .ToList();
            return answers.Where(answer => !answer.Correct)
                .Aggregate(1.0f, (current, answer) => current - 1f / correctCount);
        }

        public AnalysisResult GetData(bool scenarioEnded)
        {
            return new AnalysisResult
            {
                DifficultyLevel = DifficultyLevel,
                AnsweredQuestions = _answeredQuestionsModels,
                ScenarioEnded = scenarioEnded,
                EndDate = DateTime.Now
            };
        }
    }
}