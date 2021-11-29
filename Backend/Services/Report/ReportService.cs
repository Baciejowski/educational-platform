using System;
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

        public string TimePerSkillsGraph()
        {
            var result = new List<Array>();
            var data = _context.Sessions
                .Include(x => x.Scenario)
                .ThenInclude(x => x.Questions)
                .Where(x => x.Attempts > 0 && x.ScenarioEnded).ToList();
            foreach (var session in data)
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

                var y = session.GameplayTime / questionAmount;
                int x = 0;
                var skills = new[] { session.Vision, session.Light, session.Speed };
                for (var index = 0; index < 3; index++)
                {
                    var item = skills[index];
                    x+=(int)(Math.Round(item * 10) * Math.Pow(10, index));
                }
                result.Add(new []{x,y});
            }

            ;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.ToArray());
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