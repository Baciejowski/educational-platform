using System;
using System.Collections.Generic;
using Backend.Models;

namespace Backend.Analysis_module.Models
{
    public class AnalysisResultsModel
    {
        public double DifficultyLevel { get; set; }
        public List<AnsweredQuestion> AnsweredQuestions { get; set; }
        public bool ScenarioEnded { get; set; }
        public DateTime EndDate { get; set; }
    }
}