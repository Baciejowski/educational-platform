﻿using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.Models;

namespace Backend.Analysis_module
{
    public class StudentAnalysisModule
    {
        public double DifficultyLevel { get; set; }
        private readonly Random _random = new Random();
        private List<AnsweredQuestion> _answeredQuestionsModels = new List<AnsweredQuestion>();
        private readonly bool _randomTest = false;
        private readonly bool _randomCategorization = false;
        private readonly int _testLimit;
        private readonly float _prevRank = 0;

        public StudentAnalysisModule(int testLimit)
        {
            _testLimit = testLimit;
        }

        public StudentAnalysisModule(bool randomTest, int testLimit) : this(testLimit)
        {
            _randomTest = randomTest;
            //TO DO: get prev rank
        }
        public StudentAnalysisModule(bool randomTest, bool randomCategorization, int testLimit) : this(testLimit)
        {
            _randomTest = randomTest;
            _randomCategorization = randomCategorization;
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
            if (_randomTest || _answeredQuestionsModels.Count < _testLimit)
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
            return question.AnsweredAnswers.Where(answer => !answer.Correct)
                .Aggregate(1.0f, (current, answer) => current - 1f / correctCount);
        }

        public AnalysisResult GetData(bool scenarioEnded)
        {
            return new AnalysisResult
            {
                DifficultyLevel = DifficultyLevel,
                AnsweredQuestions = _answeredQuestionsModels,
                ScenarioEnded = scenarioEnded,
                EndDate= DateTime.Now
            };
        }
    }
}