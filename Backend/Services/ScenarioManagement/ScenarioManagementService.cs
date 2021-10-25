using System.Collections.Generic;
using System.Linq;
using Backend.Controllers.APIs;
using Backend.Models;

namespace Backend.Services.ScenarioManagement
{
    public class ScenarioManagementService : IScenarioManagementService
    {
        private List<Scenario> _scenariosList;

        public ScenarioManagementService()
        {
            _scenariosList = new List<Scenario>();//TODO: pobieranie z bazy
        }

        public void CreateScenarioFromForm(ScenarioViewModel formData, Teacher teacher)
        {
            var topic = teacher.Topics.FirstOrDefault(topic => topic.TopicName == formData.Topic);
            if (topic == null)
            {
                topic = new Topic { TopicName = formData.Topic, Teacher = teacher };
                teacher.Topics.Add(topic); //TODO: zapis do bazy
            }

            var scenario = new Scenario
            {
                Name = formData.Name,
                Topic = topic,
                Questions = new List<Question>()
            };

            foreach (var questionSet in formData.Questions)
            {
                foreach (var question in questionSet)
                {
                    var answers = new List<Answer>();
                    foreach (var answer in question.Answers)
                    {
                        if(answer.Value!= "")
                        {
                            answers.Add(new Answer()
                            {
                                Correct = answer.IsCorrect,
                                Content = answer.Value
                            });
                        }
                    }

                    scenario.Questions.Add(new Question
                    {
                        Difficulty = question.Difficulty,
                        Content = question.Content,
                        QuestionType = question.QuestionType,
                        IsImportant = question.IsImportant,
                        IsObligatory = question.Obligatory,
                        ABCDAnswers = answers
                    });
                }
            }

            _scenariosList.Add(scenario); //TODO: zapis do bazy
        }
    }
}