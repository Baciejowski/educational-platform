using System;
using System.Collections.Generic;
using System.Linq;
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
            _sessionList = new List<Session>(); //TODO: pobieranie z bazy
        }

        public IEnumerable<Class> GetClassList()
        {
            return mockClassList(); //TODO: pobieranie z bazy
        }

        public Teacher GetTeacherByAuthName(string authName)
        {
            return mockTeacherList().FirstOrDefault(x => x.AuthName == authName); //TODO: pobieranie z bazy
        }

        public IEnumerable<Topic> GetTeacherTopics(string? authName)
        {
            return mockTeacherList().FirstOrDefault(x => x.AuthName == authName)?.Topics; //TODO: pobieranie z bazy
        }

        public async void SendGameInvitationToStudents(ClassesGameInvitationViewModel classesGameInvitationViewModel)
        {
            var classItem = mockClassList()
                .FirstOrDefault(item => item.ClassID == classesGameInvitationViewModel.ClassId);
            var studentsList = mockClassList()
                .FirstOrDefault(item => item.ClassID == classesGameInvitationViewModel.ClassId)?.Students;
            var scenarioItem = mockClassList()
                .FirstOrDefault(item => item.ClassID == classesGameInvitationViewModel.ClassId)?.Teacher.Topics
                .FirstOrDefault(topicItem => topicItem.TopicID == classesGameInvitationViewModel.TopicId)?.Scenarios
                .FirstOrDefault(scenarioItem => scenarioItem.ScenarioID == classesGameInvitationViewModel.ScenarioId);
            foreach (var student in studentsList)
            {
                var gameId =
                    $"{classesGameInvitationViewModel.ClassId}-{student.StudentID}-{classesGameInvitationViewModel.TeacherId}-{classesGameInvitationViewModel.TopicId}-{classesGameInvitationViewModel.ScenarioId}-{classesGameInvitationViewModel.StartGame}-{classesGameInvitationViewModel.EndGame}";

                var code = String.Format("{0:X}", gameId.GetHashCode());
                var request = new GameInvitationRequest
                {
                    ToEmail = student.Email,
                    UserName = student.FirstName,
                    Code = code,
                    Topic = scenarioItem.Topic.TopicName,
                    Scenario = scenarioItem.Name,
                    StartDate = classesGameInvitationViewModel.StartGame.ToString("yyyy-MM-dd HH:mm"),
                    EndDate = classesGameInvitationViewModel.EndGame.ToString("yyyy-MM-dd HH:mm")
                };

                await _mailService.SendGameInvitationRequestAsync(request);
                _sessionList.Add(new Session
                {
                    Student = student,
                    Scenario = scenarioItem,
                    StartGame = classesGameInvitationViewModel.StartGame,
                    EndGame = classesGameInvitationViewModel.EndGame,
                    Code = code
                }); //TODO: zapis do bazy
            }
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

            foreach (var bioScenario in bioScenarios)
            {
                bioScenario.Topic = topics[1];
            }

            foreach (var mathScenario in mathScenarios)
            {
                mathScenario.Topic = topics[0];
            }

            var teacher = new Teacher
            {
                TeacherID = 1,
                AuthName = "google-oauth2|114172205582288262083",
                Topics = topics
            };
            var students = new[]
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
            };
            var resultClassesList = new[]
            {
                new Class
                {
                    ClassID = 1,
                    FriendlyName = "Class 1",
                    Teacher = teacher,
                    Students = students
                }
            };

            return resultClassesList;
        }

        IEnumerable<Teacher> mockTeacherList()
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

            return new[]
            {
                new Teacher
                {
                    TeacherID = 1,
                    AuthName = "google-oauth2|114172205582288262083",
                    Topics = topics
                }
            };
        }
    }
}