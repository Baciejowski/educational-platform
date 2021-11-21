using System;
using System.Collections.Generic;

namespace Backend.Analysis_module.Models
{
    public class AnalysisResult
    {
        public int AnalysisResultID { get; set; }
        public double DifficultyLevel { get; set; }
        public List<AnsweredQuestion> AnsweredQuestions { get; set; }
        public bool ScenarioEnded { get; set; }
        public DateTime EndDate { get; set; }
    }
}