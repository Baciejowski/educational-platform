using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public virtual ICollection<Class> Classes{ get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
