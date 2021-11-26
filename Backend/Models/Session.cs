using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Session
    {
        public int SessionID { get; set; }
        [JsonIgnore]
        public virtual Student Student { get; set; }
        [JsonIgnore]
        public virtual Scenario Scenario { get; set; }
        public DateTime StartGame { get; set; }
        public DateTime EndGame { get; set; }
        public string Code { get; set; }
        public bool RandomTest { get; set; }
        public bool AiCategorization { get; set; }
        public int Attempts { get; set; }

        [JsonIgnore]
        public Class Class => Student.Class;

        [JsonIgnore]
        public Teacher Teacher => Scenario.Topic.Teacher;

        [JsonIgnore]
        public Topic Topic => Scenario.Topic;


        //analysis result 
        public double DifficultyLevel { get; set; }
        public List<AnsweredQuestion> AnsweredQuestions { get; set; }
        public bool ScenarioEnded { get; set; }
        public DateTime EndDate { get; set; }

        //GameplayData

        public int Experience { get; set; }
        public int Money { get; set; }

        public int GameplayTime { get; set; }
        public float Light { get; set; }
        public float Vision { get; set; }
        public float Speed { get; set; }

    }
}