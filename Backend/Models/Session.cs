using System;

namespace Backend.Models
{
    public class Session
    {
        public Class Class { get; set; }
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public Topic Topic { get; set; }
        public Scenario Scenario { get; set; }
        public DateTime StartGame { get; set; }
        public DateTime EndGame { get; set; }
        public string Code { get; set; }
    }
}
