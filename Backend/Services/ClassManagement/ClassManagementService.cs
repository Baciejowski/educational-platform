using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Backend.Controllers.APIs;
using Backend.Models;
using Backend.Services.EmailProvider;
using Backend.Services.EmailProvider.Models;

namespace Backend.Services.ClassManagement
{
    public class ClassManagementService : IClassManagementService
    {
        private readonly IMailService _mailService;
        private List<Session> _sessionList;

        public ClassManagementService(IMailService mailService)
        {
            _mailService = mailService;
            _sessionList = new List<Session>();
        }

        public IEnumerable<Class> GetClassList()
        {
            return mockClassList();
        }

        public async void SendGameInvitationToStudents(GameViewModel gameViewModel)
        {
            var classItem = mockClassList().ToArray()
                .FirstOrDefault(classItem => classItem.ClassID == gameViewModel.ClassId);
            var studentsList = mockClassList().ToArray()
                .FirstOrDefault(classItem => classItem.ClassID == gameViewModel.ClassId)?.Students;
            var teacherItem = classItem.Teacher;
            var topicItem = mockClassList().ToArray()
                .FirstOrDefault(classItem => classItem.ClassID == gameViewModel.ClassId)?.Teacher.Topics
                .FirstOrDefault(topicItem => topicItem.TopicID == gameViewModel.TopicId);
            var scenarioItem = mockClassList().ToArray()
                .FirstOrDefault(classItem => classItem.ClassID == gameViewModel.ClassId)?.Teacher.Topics
                .FirstOrDefault(topicItem => topicItem.TopicID == gameViewModel.TopicId)?.Scenarios
                .FirstOrDefault(scenarioItem => scenarioItem.ScenarioID == gameViewModel.ScenarioId);
            foreach (var student in studentsList)
            {
                var gameId =
                    $"{gameViewModel.ClassId}-{student.StudentID}-{gameViewModel.TeacherId}-{gameViewModel.TopicId}-{gameViewModel.ScenarioId}-{gameViewModel.StartGame}-{gameViewModel.EndGame}";
                // var message = GetHashString(game);
                var code = String.Format("{0:X}", gameId.GetHashCode());
                var request = new GameInvitationRequest
                {
                    ToEmail = student.Email,
                    UserName = student.FirstName,
                    Code = code,
                    Topic = topicItem.TopicName,
                    Scenario = scenarioItem.Name,
                    StartDate = gameViewModel.StartGame.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
                    EndDate = gameViewModel.EndGame.ToLocalTime().ToString("yyyy-MM-dd HH:mm")
                };

                await _mailService.SendGameInvitationRequestAsync(request);
                _sessionList.Add(new Session
                {
                    Class = classItem,
                    Student = student,
                    Topic = topicItem,
                    Scenario = scenarioItem,
                    Teacher = teacherItem,
                    StartGame = gameViewModel.StartGame,
                    EndGame = gameViewModel.EndGame,
                    Code = code
                });
            }
        }

        private static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        IEnumerable<Class> mockClassList()
        {
            var mathScenarios = new[]
            {
                new Scenario { ScenarioID = 1, Name = "Circles" },
                new Scenario { ScenarioID = 2, Name = "Rectangles" }
            };
            var bioScenarios = new[]
            {
                new Scenario { ScenarioID = 1, Name = "Mammals" },
                new Scenario { ScenarioID = 2, Name = "Vertebrates" }
            };
            var topics = new[]
            {
                new Topic { TopicID = 1, TopicName = "Math", Scenarios = mathScenarios },
                new Topic { TopicID = 2, TopicName = "Biology", Scenarios = bioScenarios }
            };
            var teacher = new Teacher
            {
                TeacherID = 1,
                Topics = topics
            };
            var resultClassesList = new[]
            {
                new Class
                {
                    ClassID = 1,
                    FriendlyName = "Class 1",
                    Teacher = teacher,
                    Students = new[]
                    {
                        new Student
                        {
                            StudentID = 1,
                            FirstName = "Aleksandra",
                            LastName = "Swierczek",
                            Email = "ola.swierczek111@gmail.com"
                        },

                        new Student
                        {
                            StudentID = 2,
                            FirstName = "Aleksandra",
                            LastName = "Swierczek",
                            Email = "aleksandra.swierczekk@gmail.com"
                        }
                    }
                }
            };

            return resultClassesList;
        }
    }
}