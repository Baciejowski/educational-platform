using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Backend.Services
{
    public static class CommunicationWithAI
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string endpointsLocation;
        private static readonly DbContextOptionsBuilder<DataContext> optionsBuilder;
        private static readonly ConcurrentQueue<(int,string)> requestsToBeSend = new ConcurrentQueue<(int, string)>();
        private static volatile bool sendingRequests = false;

        static CommunicationWithAI()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            endpointsLocation = config.GetSection("Variables")["AIEndpointsLocation"];

            optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(config.GetSection("DbContextSettings")["ConnectionString"]);
            client.Timeout = TimeSpan.FromMinutes(25);
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
        public static void TokenizeAndAddToRequestsQueue(List<string> texts, int scenarioId, int maxLength = 300)
        {
            foreach(string file in texts)
            {
                Regex rMaxTokens = new Regex(@"(\S+\s*){1," + maxLength.ToString() + "}");
                Regex rLastSentenceEnding = new Regex("[.!?]", RegexOptions.RightToLeft);
                string temp = Regex.Replace(file.Trim(), @"\s+", " ");
                while (temp.Length > 0)
                {
                    int numberOfTokens = temp.Split().Length;
                    if (numberOfTokens < maxLength)
                    {
                        requestsToBeSend.Enqueue((scenarioId, temp.Trim()));
                        return;
                    }
                    else
                    {
                        Match m1 = rMaxTokens.Match(temp);
                        if (!m1.Success) return;
                        Match m2 = rLastSentenceEnding.Match(m1.Value);
                        if (!m2.Success)
                        {
                            requestsToBeSend.Enqueue((scenarioId, temp.Substring(m1.Index, m1.Length).TrimEnd()));
                            temp = temp[(m1.Index + m1.Length)..].TrimStart();
                        }
                        else
                        {
                            requestsToBeSend.Enqueue((scenarioId, temp.Substring(m1.Index, m2.Index + 1).TrimEnd()));
                            temp = temp[(m1.Index + m2.Index + 1)..].TrimStart();
                        }
                    }
                }
                return;
            }
        }
        public static void SendRequests()
        {
            if (sendingRequests) return;
            sendingRequests = true;
            while (!requestsToBeSend.IsEmpty)
            {
                (int, string) el;
                if (!requestsToBeSend.TryDequeue(out el)) continue;

                AiGanerateQuestionsRequest req = new AiGanerateQuestionsRequest
                {
                    ScenarioID = el.Item1,
                    Text = el.Item2
                };

                try
                {
                    AiGanerateQuestionsResponse result = JsonConvert.DeserializeObject<AiGanerateQuestionsResponse>(
                        client.PostAsync(
                            endpointsLocation + "generateQuestions",
                            new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json"))
                        .Result.Content.ReadAsStringAsync()
                        .Result);
                    using DataContext dataContext = new DataContext(optionsBuilder.Options);
                    Scenario scenario = dataContext.Scenarios.FirstOrDefault(s => s.ScenarioID == result.ScenarioID);
                    if (scenario == null) continue;

                    foreach (AiGeneratedQuestion q in result.Questions)
                    {
                        dataContext.Add(new Question
                        {
                            ABCDAnswers = new List<Answer> { new Answer { Content = q.CorrectAnswer, Correct = true } },
                            AiDifficulty = q.DifficultyLevel * (-1),
                            Content = q.Content,
                            Scenarios = new List<Scenario> { scenario },
                            QuestionType = Question.TypeEnum.ABCD
                        });
                        dataContext.SaveChanges();
                    }
                }
                catch
                {
                    continue;
                }
            }
            sendingRequests = false;
        }
    }
   public class AiGanerateQuestionsRequest
    {
        public int ScenarioID { get; set; }
        public string Text { get; set; }
    }
    public class AiGanerateQuestionsResponse
    {
        public int ScenarioID { get; set; }
        public IList<AiGeneratedQuestion> Questions { get; set; }
    }
    public class AiGeneratedQuestion
    {
        public string Content { get; set; }
        public string CorrectAnswer { get; set; }
        public int DifficultyLevel { get; set; }
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
