using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Backend.Controllers.APIs;
using Backend.Models;
using Backend.Services.EmailProvider;

namespace Backend.Services.ClassManagement
{
    public class ClassManagementService : IClassManagementService
    {
        private readonly IMailService _mailService;

        public ClassManagementService(IMailService mailService)
        {
            _mailService = mailService;
        }

        public IEnumerable<Class> GetClassList()
        {
            return mockClassList();
        }

        public async void SendGameInvitationToStudents(GameViewModel gameViewModel)
        {
            var students = mockClassList().ToArray()
                .FirstOrDefault(classItem => classItem.ClassID == gameViewModel.ClassId)?.Students;
            foreach (var student in students)
            {
                var game =
                    $"{gameViewModel.ClassId}-{student.StudentID}-{gameViewModel.TeacherId}-{gameViewModel.TopicId}-{gameViewModel.ScenarioId}-{gameViewModel.StartGame}-{gameViewModel.EndGame}";
                // var message = GetHashString(game);
                var message  = String.Format("{0:X}", game.GetHashCode());
                var request = new MailRequest
                {
                    ToEmail = student.Email,
                    Subject = "Game invitation",
                    Body = message
                };

                await _mailService.SendEmailAsync(request);
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
                            FirstName = "FirstName",
                            LastName = "LastName",
                            Email = "ola.swierczek111@gmail.com"
                        },

                        new Student
                        {
                            StudentID = 2,
                            FirstName = "FirstName",
                            LastName = "LastName",
                            Email = "aleksandra.swierczekk@gmail.com"
                        }
                    }
                }
            };

            return resultClassesList;
        }
    }
}