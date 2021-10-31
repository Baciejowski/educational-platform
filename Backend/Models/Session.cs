using System;

namespace Backend.Models
{
    public class Session
    {
        public int SessionID { get; set; }
        public Class Class => Student.Class;
        public virtual Student Student { get; set; }
        public Teacher Teacher => Scenario.Topic.Teacher;
        public Topic Topic => Scenario.Topic;
        public virtual Scenario Scenario { get; set; }
        public DateTime StartGame { get; set; }
        public DateTime EndGame { get; set; }
        public string Code { get; set; }
    }
}
