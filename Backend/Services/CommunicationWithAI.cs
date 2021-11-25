using Backend.Models;
using Microsoft.Extensions.Configuration;
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
        private static readonly string endpointsLocation;
        static CommunicationWithAI()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            endpointsLocation = config.GetSection("Variables")["AIEndpointsLocation"];
        }
        public static void UpdateProposedDifficulty(DataContext context, Scenario scenario)
        {
            AiScenario result;
            try { 
                result = JsonConvert.DeserializeObject<AiScenario>(
                    client.PostAsync(
                        endpointsLocation + "difficulty", 
                        new StringContent(scenario.AIRespresentation, Encoding.UTF8, "application/json"))
                    .Result.Content.ReadAsStringAsync()
                    .Result);
            }
            catch 
            {
                return;
            }
            foreach (AiQuestion question in result.Questions)
            {
                Question original = scenario.Questions.FirstOrDefault(q => q.QuestionID == question.QuestionID);
                if (original == null) continue;
                original.AiDifficulty = (-1)*question.DifficultyLevel;
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
