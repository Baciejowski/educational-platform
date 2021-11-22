using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Analysis_module.AdaptivityModule;
using Backend.Analysis_module.Models;
using Backend.Models;
using Gameplay;
using Microsoft.EntityFrameworkCore;

namespace Backend.Analysis_module.SessionModule
{
    public class SessionModuleService
    {
        protected readonly DataContext Context;
        public StudentData StudentData { get; set; }

        private readonly Session _userSession;
        private Question[] _readyQuestions;
        private List<Question>[] _availableQuestions;
        private AdaptivityModuleService _adaptivityModuleService;
        private static readonly Random Random = new Random();
        private bool _questionAsked = false;
        private readonly DateTime _startDate;
        private DateTime _lastRequest;
        private const int AllowedAfkTime = 30;


        public SessionModuleService(string studentEmail, int studentId, string code, string sessionId,
            DataContext context)
        {
            Context = context;
            StudentData = new StudentData
            {
                Email = studentEmail.ToLower(),
                StudentId = studentId,
                Code = code.ToLower(),
                SessionId = sessionId
            };
            _availableQuestions = GetQuestionsFromScenario();
            _readyQuestions = SetInitialQuestions();
            _adaptivityModuleService = new AdaptivityModuleService(TestLimit());
            _startDate = DateTime.Now;
            _userSession = Context.Scenarios.Include(x => x.Sessions).FirstOrDefault(x => x.Name == "TestRectangles")
                ?.Sessions.First();
            SetRequestTime();
        }

        public SessionModuleService(string studentEmail, int studentId, string code, string sessionId,
            Session userSession, DataContext context)
        {
            StudentData = new StudentData
            {
                Email = studentEmail.ToLower(),
                StudentId = studentId,
                Code = code.ToLower(),
                SessionId = sessionId
            };
            _userSession = userSession;
            Context = context;


            _availableQuestions = GetQuestionsFromScenario();
            _readyQuestions = SetInitialQuestions();
            _adaptivityModuleService =
                new AdaptivityModuleService(_userSession.RandomTest, TestLimit());
            _startDate = DateTime.Now;
            SetRequestTime();
        }

        public Question GetQuestion(QuestionImportanceType questionImportanceType)
        {
            _questionAsked = true;
            SetRequestTime();
            return _readyQuestions[(int)questionImportanceType];
        }

        public IEnumerable<int> GetQuestionsAmount()
        {
            int[] index = { 0, 1, 2 };
            SetRequestTime();
            return index.Select(GetQuestionAmount);
        }

        public void SaveAnswerResponse(StudentAnswerRequest request)
        {
            if (!_questionAsked) return;
            _questionAsked = false;

            var answeredQuestion = StudentResponseAdapter(request);
            _adaptivityModuleService.AddQuestionToAnalysis(answeredQuestion);
            FindNextQuestion(answeredQuestion.QuestionImportanceType);
            SetRequestTime();
        }

        public QuestionResponse.Types.QuestionReward CalculateReward()
        {
            SetRequestTime();
            return new QuestionResponse.Types.QuestionReward
            {
                Money = 100,
                Experience = 100
            }; //To Do  - oblicza
        }

        public bool EndGame(EndGameRequest request)
        {
            var analysisResult = _adaptivityModuleService.GetData(request.ScenarioEnded);
            var gameplayData = new GameplayData
            {
                Experience = request.StudentEndGameData.Experience,
                Money = request.StudentEndGameData.Money,
                GameplayTime = request.GameplayTime,
                Light = request.StudentEndGameData.Light,
                Vision = request.StudentEndGameData.Vision,
                Speed = request.StudentEndGameData.Speed
            };
            Context.AnalysisResults.Add(analysisResult);
            Context.SaveChanges();

            var result = new SessionRecord
            {
                AnalysisResult = new AnalysisResult(),
                Session = new Session(),
                GameplayData = new GameplayData()
            };
            // try
            // {
            //     Context.SessionRecords.Add(result);
            //     Context.SaveChanges();
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw;
            // }

            SetRequestTime();

            return true;
        }

        public bool IsAbandoned()
        {
            var afkTime = (DateTime.Now - _lastRequest).TotalMinutes;

            if (afkTime < AllowedAfkTime) return false;

            var gameTime = (int)Math.Round((_lastRequest - _startDate).TotalSeconds);
            var result = new EndGameRequest
            {
                ScenarioEnded = false,
                GameplayTime = gameTime,
                StudentEndGameData = new EndGameRequest.Types.StudentEndGameData
                    { Experience = 0, Money = 0 }
            };
            EndGame(result);
            return true;
        }

        private int GetQuestionAmount(int questionImportanceIndex)
        {
            var type = _availableQuestions[questionImportanceIndex];
            return type
                .GroupBy(x => x.Difficulty)
                .OrderBy(x => x.Count())
                .First().Count();
        }

        private AnsweredQuestion StudentResponseAdapter(StudentAnswerRequest request)
        {
            var type = (QuestionImportanceType)((int)request.QuestionType);
            var question = GetQuestion(type);
            var answersId = request.AnswersID.ToList();
            var answers = question.ABCDAnswers.Where(x => answersId.Contains(x.AnswerID)).ToList();
            return new AnsweredQuestion
            {
                QuestionImportanceType = type,
                Question = question,
                AnsweredAnswers = request.AnswersID.ToList(),
                TimeToAnswer = request.TimeToAnswer
            };
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
            return StudentData.Email == "test" && StudentData.Code == "test";
        }

        private void SetRequestTime()
        {
            _lastRequest = DateTime.Now;
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
            var index = Random.Next(_availableQuestions[questionIndex].Count);
            return _availableQuestions[questionIndex].ElementAt(index);
        }

        private Question GetRankedQuestionOfType(int questionIndex)
        {
            var nextQuestionDifficulty = _adaptivityModuleService.GetNextQuestionDifficulty();
            var filteredQuestions = _availableQuestions[questionIndex]
                .Where(x => x.Difficulty == nextQuestionDifficulty).ToList();
            var index = Random.Next(filteredQuestions.Count);
            return filteredQuestions.ElementAt(index);
        }

        private void FindNextQuestion(QuestionImportanceType questionImportanceType)
        {
            var questionIndex = (int)questionImportanceType;
            RemoveQuestion(questionIndex);
            _readyQuestions[questionIndex] =
                _adaptivityModuleService.DifficultyLevel < 0
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
                var difficulty = (byte)(j % 2 + 1);
                var newQuestion = new Question
                {
                    Content = data[0],
                    IsObligatory = type == 0,
                    IsImportant = type == 1,
                    Difficulty = difficulty,
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
    }
}