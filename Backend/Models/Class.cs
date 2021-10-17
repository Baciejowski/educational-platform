using System.Collections.Generic;

namespace Backend.Models
{
    public class Class
    {
        public int ClassID { get; set; }
        public string FriendlyName { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
