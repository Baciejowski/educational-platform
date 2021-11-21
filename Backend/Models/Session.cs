using System;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Session
    {
        public int SessionID { get; set; }
        public virtual Student Student { get; set; }
        public virtual Scenario Scenario { get; set; }
        public DateTime StartGame { get; set; }
        public DateTime EndGame { get; set; }
        public string Code { get; set; }
        public bool RandomTest { get; set; }
        public bool RandomCategorization { get; set; }
        public int Attempts { get; set; }

        [JsonIgnore]
        public Class Class => Student.Class;

        [JsonIgnore]
        public Teacher Teacher => Scenario.Topic.Teacher;

        [JsonIgnore]
        public Topic Topic => Scenario.Topic;

        [JsonIgnore]
        public SessionRecord SessionRecord { get; set; }
    }
}
