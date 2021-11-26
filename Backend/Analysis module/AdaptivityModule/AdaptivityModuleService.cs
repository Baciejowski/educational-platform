using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.Models;
using Backend.Models;
using Gameplay;

namespace Backend.Analysis_module.AdaptivityModule
{
    public class AdaptivityModuleService
    {
        public double DifficultyLevel { get; set; }
        public bool AiCategorization { get; set; }
        public bool RandomTest { get; set; }
        public double PrevDifficulty { get; set; }

        private readonly Random _random = new Random();
        private List<AnsweredQuestion> _answeredQuestionsModels = new List<AnsweredQuestion>();
        private readonly int _testLimit;

        public AdaptivityModuleService(int testLimit)
        {
            _testLimit = testLimit;
            AiCategorization = false;
            RandomTest = false;
            PrevDifficulty = 0;
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
                var value = PrevDifficulty;
                var divider = 1;
                for (var i = 0; i < _answeredQuestionsModels.Count; i++)
                {
                    value += (i + 1) * _answeredQuestionsModels[i].Correctness *
                             _answeredQuestionsModels[i].Difficulty;
                    divider += (i + 1);
                }

                DifficultyLevel = Math.Round(Math.Max(value / (float)divider, 1) * 1000) / 1000;
            }
        }

        public void AddQuestionToAnalysis(AnsweredQuestion question)
        {
            _answeredQuestionsModels.Add(question);
            CalculateLvl();
        }

        public float CalculateCorrectness(StudentAnswerRequest request, Question question)
        {
            var correctCount = question.ABCDAnswers.Count(x => x.Correct);
            var answers = question.ABCDAnswers.Where(x => request.AnswersID.Contains(x.AnswerID))
                .ToList();
            return answers.Where(answer => !answer.Correct)
                .Aggregate(1.0f, (current, answer) => current - 1f / correctCount);
        }

        public AnalysisResultsModel GetData(bool scenarioEnded)
        {
            return new AnalysisResultsModel
            {
                DifficultyLevel = DifficultyLevel,
                AnsweredQuestions = _answeredQuestionsModels,
                ScenarioEnded = scenarioEnded,
                EndDate = DateTime.Now
            };
        }
    }
}