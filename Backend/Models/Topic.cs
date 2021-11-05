using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Topic
    {
        public int TopicID { get; set; }
        public string TopicName { get; set; }
        public virtual ICollection<Scenario> Scenarios { get; set; }
        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }
    }
}
