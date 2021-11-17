using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Scenario
    {
        public int ScenarioID { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Question> Questions { get; set; }

        [JsonIgnore]
        public virtual Topic Topic { get; set; }

        [JsonIgnore]
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public string AIRespresentation
        {
            get
            {
                string res = $"{{\"ScenarioID\":{ScenarioID},\"Questions\":[";
                string questions = "";
                foreach (Question q in Questions)
                    questions += q.AIRespresentation + ",";
                if (questions.Length > 0)
                {
                    return res + questions.Substring(0, questions.Length - 1) + "]}";
                }
                return res + "]}";
            }
        }
    }
}
