using System.Collections.Generic;

namespace Backend.Models
{
    public class Topic
    {
        public int TopicID { get; set; }
        public string TopicName { get; set; }
        public virtual ICollection<Scenario> Scenarios { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
