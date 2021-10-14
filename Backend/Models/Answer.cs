using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public bool Correct { get; set; }
        public string Content { get; set; }
        public string Argumentation { get; set; }
        public virtual Question Question { get; set; }
    }
}
