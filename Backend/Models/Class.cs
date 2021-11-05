using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Class
    {
        public int ClassID { get; set; }
        public string FriendlyName { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }
    }
}
