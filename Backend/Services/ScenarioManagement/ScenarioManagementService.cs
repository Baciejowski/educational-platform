using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Controllers.APIs;
using Backend.Models;
using Newtonsoft.Json;

namespace Backend.Services.ScenarioManagement
{
    public class ScenarioManagementService : IScenarioManagementService
    {
        private List<Scenario> _scenariosList;

        public ScenarioManagementService()
        {
            _scenariosList = new List<Scenario>();
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
                    foreach (var answer in question.answers)
                    {
                        answers.Add(new Answer()
                        {
                            Correct = answer.isCorrect,
                            Content = answer.value
                        });
                    }

                    scenario.Questions.Add(new Question
                    {
                        Difficulty = question.difficulty,
                        Content = question.content,
                        QuestionType = question.questionType,
                        IsImportant = question.isImportant,
                        IsObligatory = question.obligatory,
                        ABCDAnswers = answers
                    });
                }
            }

            _scenariosList.Add(scenario); //TODO: zapis do bazy
        }
    }
}