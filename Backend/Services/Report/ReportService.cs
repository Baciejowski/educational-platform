using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Report
{
    public class ReportService : IReportService
    {
        private DataContext _context;
        GroupSettings[] _groupSettings;

        public ReportService(DataContext context)
        {
            _context = context;
            _groupSettings = new[]
            {
                new GroupSettings("No adaptivity", true, false),
                new GroupSettings("Basic adaptivity", false, false),
                new GroupSettings("Advance adaptivity", false, true),
            };
        }

        public GeneralReportDto GetAllGeneralGraphs()
        {
            return new GeneralReportDto
            {
                Participation = ParticipationGraph(),
                TimePerAttempt = TimePerAttemptGraph(),
                TimePerSkills = TimePerSkillsGraph(),
                ScenarioResults = ScenarioResultsGraph(),
                AvgTimePerScenario = AvgTimePerScenarioGraph(),
                AvgAnsweredQuestionsPerScenario = AvgAnsweredQuestionsPerScenarioGraph(),
                SuccessPerScenario = SuccessPerScenarioGraph(),
                ScenarioResultsPerGroup = ScenarioResultsPerGroupGraph(),
                DifficultyScaling = DifficultyScalingGraph()
            };
        }

        private string CheckGroup(Session session)
        {
            return _groupSettings.FirstOrDefault(x =>
                session.AiCategorization == x.AiDifficulty && session.RandomTest == x.RandomTest)?.Label;
        }

        private int QuestionsAmountPerScenarioByGroup(Session session)
        {
            var questionAmount = 0;
            var questions = session.Scenario.Questions;
            for (var i = 0; i < 3; i++)
            {
                var type = i switch
                {
                    0 => questions.Where(x => x.IsObligatory && x.QuestionType == Question.TypeEnum.ABCD).ToList(),
                    1 => questions.Where(x => x.IsImportant && x.QuestionType == Question.TypeEnum.ABCD).ToList(),
                    2 => questions.Where(x =>
                            !x.IsObligatory && !x.IsImportant && x.QuestionType == Question.TypeEnum.ABCD)
                        .ToList()
                };
                if (session.RandomTest || i == 0)
                    questionAmount += type.Count;

                else if (session.AiCategorization)
                    questionAmount += type
                        .GroupBy(x => Math.Abs(x.AiDifficulty ?? 1))
                        .OrderBy(x => x.Count())
                        .First().Count();
                else
                    questionAmount += type
                        .GroupBy(x => x.Difficulty)
                        .OrderBy(x => x.Count())
                        .First().Count();
            }

            return questionAmount;
        }

        public string TimePerSkillsGraph()
        {
            var result = new List<Array>();
            var data = _context.Sessions
                .Include(x => x.Scenario)
                .ThenInclude(x => x.Questions)
                .Where(x => x.Attempts > 0 && x.ScenarioEnded).ToList();
            foreach (var session in data)
            {
                var questionAmount = QuestionsAmountPerScenarioByGroup(session);
                var y = session.GameplayTime / questionAmount;
                int x = 0;
                var skills = new[] { session.Vision, session.Light, session.Speed };
                for (var index = 0; index < 3; index++)
                {
                    var item = skills[index];
                    x += (int)(Math.Round(item * 100) * Math.Pow(100, index));
                }

                result.Add(new[] { x, y });
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.ToArray());
            return json;
        }

        public string TimePerAttemptGraph()
        {
            var result = new List<object>
            {
                new { name = "No adaptivity", data = new List<Array>() },
                new { name = "Basic adaptivity", data = new List<Array>() },
                new { name = "Advance adaptivity", data = new List<Array>() }
            };
            var sessionData = _context.Sessions.Include(x => x.Student)
                .Where(x => x.ScenarioEnded)
                .AsParallel()
                .ToLookup(x => x.Student.StudentID).ToList();
            foreach (var grouping in sessionData)
            {
                var name = CheckGroup(grouping.First());
                var i = 1;
                foreach (var session in grouping)
                {
                    result.Cast<dynamic>().FirstOrDefault(x => x.name == name)?.data
                        .Add(new[] { i, session.GameplayTime });
                    i++;
                }
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.ToArray());
            return json;
        }


        public string DifficultyScalingGraph()
        {
            var result = new List<object>
            {
                new { name = "Basic adaptivity", data = new List<Array>() },
                new { name = "Advance adaptivity", data = new List<Array>() }
            };
            var sessionData = _context.Sessions.Include(x => x.Student)
                .Where(x => x.ScenarioEnded)
                .AsParallel()
                .ToLookup(x => x.Student.StudentID).ToList();
            foreach (var grouping in sessionData)
            {
                var name = CheckGroup(grouping.First());
                var i = 1;
                foreach (var session in grouping)
                {
                    if (name == "No adaptivity") continue;
                    result.Cast<dynamic>().FirstOrDefault(x => x.name == name)?.data
                        .Add(new[] { i, session.DifficultyLevel });
                    i++;
                }
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.ToArray());
            return json;
        }

        public string ParticipationGraph()
        {
            var sessionData = _context.Sessions.Count();
            var endedSessions = _context.Sessions.Count(x => x.Attempts > 0);
            var result = new List<object>
            {
                new { name = "Pending", data = sessionData - endedSessions },
                new { name = "Finished", data = endedSessions }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return json;
        }

        public string ScenarioResultsPerGroupGraph()
        {
            var groupedSessions = _context.Sessions.Include(x => x.Student)
                .Where(x => x.Attempts > 0)
                .AsParallel()
                .ToLookup(CheckGroup);

            var result = new List<object>();
            foreach (var group in groupedSessions)
            {
                ;
                var endedSessions = 0;
                var succesedSessions = 0;
                foreach (var session in @group)
                {
                    endedSessions++;
                    if (session.ScenarioEnded)
                        succesedSessions++;
                }

                result.Add(new
                {
                    name = group.Key,
                    fail = endedSessions - succesedSessions,
                    success = succesedSessions
                });
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return json;
        }

        public string  ScenarioResultsGraph()
        {
            var endedSessions = _context.Sessions.Count(x => x.Attempts > 0);
            var succesedSessions = _context.Sessions.Count(x => x.Attempts > 0 && x.ScenarioEnded);
            var result = new List<object>
            {
                new { name = "Fail", data = endedSessions - succesedSessions },
                new { name = "Success", data = succesedSessions }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return json;
        }

        public string AvgTimePerScenarioGraph()
        {
            var result = new List<object>();
            var groupedSessions = _context.Sessions.Include(x => x.Scenario).Where(x => x.ScenarioEnded).AsParallel()
                .ToLookup(x => x.Scenario.Name).ToList();
            foreach (var group in groupedSessions)
            {
                var count = 0;
                var sum = 0;
                foreach (var session in group)
                {
                    count++;
                    sum += session.GameplayTime;
                }

                var total = Math.Round((double)(sum / count));
                var avg = new double[3];
                for (var index = 0; index < _groupSettings.Length; index++)
                {
                    var groupSettings = _groupSettings[index];
                    count = 0;
                    sum = 0;

                    foreach (var session in group)
                    {
                        if (session.RandomTest == groupSettings.RandomTest &&
                            session.AiCategorization == groupSettings.AiDifficulty)
                        {
                            count++;
                            sum += session.GameplayTime;
                        }
                    }

                    avg[index] = count > 0 ? Math.Round((double)(sum / count)) : count;
                }

                result.Add(new { name = group.Key, total = total, random = avg[0], basic = avg[1], ai = avg[2] });
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return json;
        }

        public string AvgAnsweredQuestionsPerScenarioGraph()
        {
            var result = new List<object>();
            var groupedSessions = _context.Sessions
                .Include(x => x.Scenario)
                .Include(x => x.AnsweredQuestions)
                .Where(x => x.ScenarioEnded)
                .AsParallel()
                .ToLookup(x => x.Scenario.Name).ToList();

            foreach (var group in groupedSessions)
            {
                var avg = new double[3];
                for (var index = 0; index < _groupSettings.Length; index++)
                {
                    var groupSettings = _groupSettings[index];
                    var count = 0;
                    var sum = 0;

                    foreach (var session in group)
                    {
                        if (session.RandomTest == groupSettings.RandomTest &&
                            session.AiCategorization == groupSettings.AiDifficulty)
                        {
                            count += QuestionsAmountPerScenarioByGroup(session);
                            sum += session.AnsweredQuestions.Count;
                        }
                    }

                    avg[index] = count > 0 ? Math.Round((sum / (double)count) * 100) : count;
                }

                result.Add(new { name = group.Key, random = avg[0], basic = avg[1], ai = avg[2] });
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return json;
        }

        public string SuccessPerScenarioGraph()
        {
            var result = new List<object>();
            var groupedSessions = _context.Sessions
                .AsParallel()
                .ToLookup(x => x.Scenario.Name).ToList();

            foreach (var group in groupedSessions)
            {
                var avg = new double[3];
                for (var index = 0; index < _groupSettings.Length; index++)
                {
                    var groupSettings = _groupSettings[index];
                    var count = 0;
                    var sum = 0;

                    foreach (var session in group)
                    {
                        if (session.RandomTest == groupSettings.RandomTest &&
                            session.AiCategorization == groupSettings.AiDifficulty)
                        {
                            count++;
                            if (session.ScenarioEnded)
                                sum++;
                        }
                    }

                    avg[index] = count > 0 ? Math.Round((sum / (double)count) * 100) : count;
                }

                result.Add(new { name = group.Key, random = avg[0], basic = avg[1], ai = avg[2] });
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return json;
        }
    }


    public class GroupSettings
    {
        public GroupSettings(string label, bool randomTest, bool aiDifficulty)
        {
            Label = label;
            RandomTest = randomTest;
            AiDifficulty = aiDifficulty;
        }

        public string Label { get; set; }
        public bool RandomTest { get; set; }
        public bool AiDifficulty { get; set; }
    }
}