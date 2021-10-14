using System.Collections.Generic;

namespace Backend.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
