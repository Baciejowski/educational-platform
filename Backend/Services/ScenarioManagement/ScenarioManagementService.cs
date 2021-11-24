using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers.APIs;
using Backend.Controllers.APIs.Models;
using Backend.Models;
using Backend.Services;

namespace Backend.Services.ScenarioManagement
{
    public class ScenarioManagementService : IScenarioManagementService
    {
        private readonly DataContext _context;

        public ScenarioManagementService(DataContext context)
        {
            _context = context;
        }

        public async void CreateScenarioFromForm(ScenarioViewModel formData, Teacher teacher)
        {
            var topic = teacher.Topics.FirstOrDefault(topic => topic.TopicName == formData.Topic);
            if (topic == null)
            {
                topic = new Topic { TopicName = formData.Topic, Teacher = teacher };
                _context.Topics.Add(topic);
            }

            var checkScenario = _context.Scenarios.FirstOrDefault(x => x.Name == formData.Name);
            var checkTopic = _context.Topics.FirstOrDefault(x => x.TopicID == topic.TopicID);
            if (checkScenario != null && checkTopic != null) return;

            var scenario = new Scenario
            {
                Name = formData.Name,
                Url = formData.Url,
                Topic = topic,
                Questions = new List<Question>()
            };

            foreach (var questionSet in formData.Questions)
            {
                foreach (var question in questionSet)
                {
                    if (question.Difficulty == 6) continue;
                    var answers = new List<Answer>();
                    foreach (var answer in question.Answers)
                    {
                        if (answer.Value != "")
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

            _context.Scenarios.Add(scenario);
            _context.SaveChanges();
            Task.Run(() => CommunicationWithAI.UpdateProposedDifficulty(_context, scenario));
        }
    }
}