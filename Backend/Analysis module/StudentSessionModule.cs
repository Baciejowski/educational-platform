﻿using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.Models;
using Backend.Models;
using Gameplay;

namespace Backend.Analysis_module
{
    public class StudentSessionModule : IStudentSessionModule
    {
        protected readonly DataContext Context;
        public StudentSessionData StudentSessionData { get; set; }
        public string Email { get; set; }
        public int StudentId { get; set; }
        public string SessionId { get; set; }
        public string Code { get; set; }

        private readonly Session _userSession;
        private Question[] _readyQuestions;
        private List<Question>[] _availableQuestions;
        private StudentAnalysisModule _studentAnalysisModule;
        private static readonly Random _random = new Random();
        private bool _questionAsked = false;

        public StudentSessionModule(string studentEmail, int studentId, string code, string sessionId, DataContext context)
        {
            Context = context;
            StudentSessionData = new StudentSessionData
            {
                Email = studentEmail,
                StudentId = studentId,
                Code = code,
                SessionId = sessionId
            };
            _availableQuestions = GetQuestionsFromScenario();
            _readyQuestions = SetInitialQuestions();
            _studentAnalysisModule = new StudentAnalysisModule(TestLimit());
        }

        public StudentSessionModule(string studentEmail, int studentId, string code, string sessionId,
            Session userSession, DataContext context)
        {
            Email = studentEmail;
            StudentId = studentId;
            Code = code;
            SessionId = sessionId;
            _userSession = userSession;
            Context = context;


            _availableQuestions = GetQuestionsFromScenario();
            _readyQuestions = SetInitialQuestions();
            _studentAnalysisModule =
                new StudentAnalysisModule(_userSession.RandomTest, TestLimit());
        }

        public Question GetQuestion(QuestionImportanceType questionImportanceType)
        {
            _questionAsked = true;
            return _readyQuestions[(int)questionImportanceType];
        }

        public IEnumerable<int> GetQuestionsAmount()
        {
            int[] index = { 0, 1, 2 };
            return index.Select(GetQuestionAmount);
        }

        private int GetQuestionAmount(int questionImportanceIndex)
        {
            return _availableQuestions[questionImportanceIndex]
                .GroupBy(x => x.Difficulty)
                .Select(group => group.Count())
                .OrderByDescending(x => x)
                .First();
        }

        public void SaveAnswerResponse(AnsweredQuestionModel answeredQuestion)
        {
            if (!_questionAsked) return;
            _questionAsked = false;

            answeredQuestion.Question = GetQuestion(answeredQuestion.QuestionImportanceType);
            _studentAnalysisModule.AddQuestionToAnalysis(answeredQuestion);
            FindNextQuestion(answeredQuestion.QuestionImportanceType);
        }

        public QuestionResponse.Types.QuestionReward CalculateReward()
        {
            return new QuestionResponse.Types.QuestionReward
            {
                Money = 100,
                Experience = 100
            }; //To Do  - oblicza
        }

        private Question[] SetInitialQuestions()
        {
            var result = new Question[3];
            for (var i = 0; i < 3; i++)
            {
                result[i] = GetRandomQuestionOfType(i);
            }

            return result;
        }

        private int TestLimit()
        {
            return (int)Math.Round(_availableQuestions[0].Count * 0.3);
        }

        private bool IsTestUser()
        {
            return Email == "test" && Code == "test";
        }

        private List<Question>[] GetQuestionsFromScenario()
        {
            var result = new List<Question>[3];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new List<Question>();
            }

            var scenario = IsTestUser() ? MockScenario() : _userSession.Scenario.Questions;
            foreach (var question in scenario)
            {
                if (question.IsObligatory)
                    result[0].Add(question);
                else if (question.IsImportant)
                    result[1].Add(question);
                else
                    result[2].Add(question);
            }

            return result;
        }

        private Question GetRandomQuestionOfType(int questionIndex)
        {
            var index = _random.Next(_availableQuestions[questionIndex].Count);
            return _availableQuestions[questionIndex].ElementAt(index);
        }

        private Question GetRankedQuestionOfType(int questionIndex)
        {
            var nextQuestionDifficulty = _studentAnalysisModule.GetNextQuestionDifficulty();
            var filteredQuestions = _availableQuestions[questionIndex]
                .Where(x => x.Difficulty == nextQuestionDifficulty).ToList();
            var index = _random.Next(filteredQuestions.Count);
            return filteredQuestions.ElementAt(index);
        }

        private void FindNextQuestion(QuestionImportanceType questionImportanceType)
        {
            var questionIndex = (int)questionImportanceType;
            RemoveQuestion(questionIndex);
            _readyQuestions[questionIndex] =
                _studentAnalysisModule.DifficultyLevel < 0
                    ? GetRandomQuestionOfType(questionIndex)
                    : GetRankedQuestionOfType(questionIndex);
        }

        private void RemoveQuestion(int questionIndex)
        {
            _availableQuestions[questionIndex].Remove(_readyQuestions[questionIndex]);
        }

        private static List<Question> MockScenario()
        {
            string[] scenario =
            {
                "Who was the first president of United States of America?;George Washington;Thomas Jefferson;Abraham Lincoln;Benjamin Franklin;1",
                "What is the largest big cat?;Lion;Tiger;Cheetah;Leopard;2",
                "What land animal can open its mouth the widest?;Alligator;Crocodile;Baboon;Hippo;4",
                "What is the largest animal on Earth?; The African elephant; The blue whale; The sperm whale; The giant squid; 2",
                "What is the only flying mammal?; The bat; The flying squirrel; The bald eagle; The colugo; 1",
                "What is an animal called that eats plants and meat?;Carnivore;Herbivore;Omnivore;Pescatarian;3",
                "Why do sea otters hold hands?;Because they love each other;To show they’re in the same family;So they don’t float away when they’re sleeping;Because they’re playing;3",
                "How can you tell an insect and a spider apart?;Insects have three body parts, spiders have two.;Insects have six legs, spiders have eight.;Insects can have wings but spiders can’t.;All of the above.;4",
                "What does the duck-billed platypus do that hardly any other mammals do?;Quacks like a duck; Lays eggs; Builds nests; Waddles; 2",
                "Why do snakes stick out their tongue ?; To scare predators; To lick their prey; To make a hissing sound; To “smell” the air; 4",
                "What is it called when there are no more of one kind of animal left on Earth ?; Evolution; Conservation; Extinction; Endangered; 3",
                "What’s the biggest planet in our solar system ?; Jupiter; Saturn; Neptune; Mercury; 1",
                "What planet has the shortest day?;Mercury;Earth;Neptune;Jupiter;4",
                "What star is closest to the Earth?;The North Star (Polaris);The Dog Star (Sirius);The sun;Andromeda;3",
                "Who was the first person to walk on the moon?;Buzz Aldrin;Neil Armstrong;Michael Collins;Alan Shepard;2",
                "What’s a blue moon?; When the moon turns blue; When the moon falls on Halloween; The second full moon in a month; When a Hunter’s Moon falls on Halloween; 3",
                "What’s a lunar eclipse?;When the Earth is in between the sun and the moon;When the moon is in between the Earth and the sun;When the sun is in between the Earth and the moon;When the moon is closest to the Earth;1",
                "What direction does the sun rise in?;North;South;East;West;3",
                "Does ice sink or float in water?;Sink;Float;Sometimes it sinks, sometimes it floats;Ice is water, so this is a trick question;2",
                "What’s the force that makes objects fall to the ground?;Electromagnetism;Gravity;Nuclear force;It’s just called “The Force”;2",
                "What kind of trees grow from acorns?;Oak;Maple;Hickory;Walnut;1",
                "What’s the difference between a hurricane and a typhoon?;Typhoons are stronger than hurricanes.;Typhoons happen over land, hurricanes over water.;Hurricanes are slower-moving.;Nothing except where they happen.;4",
                "How many colors are in a rainbow?;7;10;6;8;1",
                "What’s the hardest substance in our body?; Bones; Hair; Nails; Teeth; 4",
                "How many bones are in the adult human body?;501;105;206;347;3",
                "Where is the fastest muscle in the body?;The leg;The arm;The fingers;The eye;4",
                "What is the largest body of water on Earth?;The Pacific Ocean;The Atlantic Ocean;The Indian Ocean;The Caspian Sea;1",
                "What is the largest country on Earth?;The United States;Canada;China;Russia;4",
                "What is the smallest country on Earth?;Monaco;Luxembourg;Vatican City;Madagascar;3",
                "What is the coldest place on Earth?;The North Pole;Antarctica;Siberia;Cape Horn, South America;2"
            };
            var result = new List<Question>();
            for (var j = 0; j < scenario.Length; j++)
            {
                var question = scenario[j];
                var data = question.Split(";");
                var correctAnswer = Convert.ToInt32(data.Last());
                var type = j % 3;
                var difficulty = (byte)_random.Next(0, 6);
                var newQuestion = new Question
                {
                    Content = data[0],
                    IsObligatory = type == 0,
                    IsImportant = type == 1,
                    Difficulty = 1,
                    ABCDAnswers = new List<Answer>()
                };
                for (var i = 1; i < data.Length - 1; i++)
                {
                    newQuestion.ABCDAnswers.Add(
                        new Answer
                        {
                            AnswerID = i,
                            Content = data[i],
                            Correct = correctAnswer == (i)
                        });
                }

                result.Add(newQuestion);
            }

            return result;
        }

        public void EndGame(EndGameRequest request)
        {
            // DataContext.
            throw new NotImplementedException();
        }
    }
}