using Backend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    public static class CommunicationWithAI
    {
        private static readonly HttpClient client = new HttpClient();
        public static void UpdateProposedDifficulty(DataContext context, Scenario scenario)
        {
            AiScenario result = JsonConvert.DeserializeObject<AiScenario>(
                client.PostAsync(
                    "http://localhost:8000/difficulty", 
                    new StringContent(scenario.AIRespresentation, Encoding.UTF8, "application/json"))
                .Result.Content.ReadAsStringAsync()
                .Result);
            foreach (AiQuestion question in result.Questions)
            {
                Question original = scenario.Questions.FirstOrDefault(q => q.QuestionID == question.QuestionID);
                if (original == null) continue;
                original.AiDifficulty = question.DifficultyLevel;
                context.Update(original);
            }
            context.SaveChanges();
        }
    }
    public class AiScenario
    {
        public int ScenarioID { get; set; }
        public IList<AiQuestion> Questions { get; set; }
    }
    public class AiQuestion
    {
        public int QuestionID { get; set; }
        public int DifficultyLevel { get; set; }
    }
}
