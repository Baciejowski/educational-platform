using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class AnalysisResult
    {
        public int AnalysisResultID { get; set; }
        public double DifficultyLevel { get; set; }
        public List<AnsweredQuestion> AnsweredQuestions { get; set; }
        public bool ScenarioEnded { get; set; }
        public DateTime EndDate { get; set; }

        [JsonIgnore]
        public virtual SessionRecord SessionRecord { get; set; }

        [ForeignKey("SessionRecord")]
        public int SessionRecordID { get; set; }
    }
}