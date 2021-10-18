using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services.ClassManagement
{
    public class ClassManagementService: IClassManagementService
    {
        public IEnumerable<Class> GetMockedClassList()
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
                new Topic { TopicID = 2, TopicName = "Biology", Scenarios = bioScenarios}
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
                            Email = "ola.swierczek111@gmail.com"
                        },

                        new Student
                        {
                            StudentID = 2,
                            Email = "ola.swierczek111@gmail.com"
                        }
                    }
                }
            };

            return resultClassesList;
        }
    }
}
