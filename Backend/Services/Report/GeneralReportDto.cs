﻿namespace Backend.Services.Report
{
    public class GeneralReportDto
    {
        public string TimePerSkills { get; set; }
        public string TimePerAttempt { get; set; }
        public string Participation { get; set; }
        public string ScenarioResults { get; set; }
        public string AvgTimePerQuestion { get; set; }
        public string AvgAnsweredQuestionsPerScenario { get; set; }
        public string SuccessPerScenario { get; set; }
        public string ScenarioResultsPerGroup { get; set; }
        public string DifficultyScaling { get; set; }
        public string AttemptsPerScenario { get; set; }
        public string MedianAnsweredQuestionsPerScenario { get; set; }
        public string MedianTimePerQuestion { get; set; }
        public string AvgDifficultyScaling { get; set; }
    }
}