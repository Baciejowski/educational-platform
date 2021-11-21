using System.Collections.Generic;
using Backend.Analysis_module.Models;

namespace Backend.Models
{
    public class AnsweredQuestion
    {
        public int AnsweredQuestionID { get; set; }
        public QuestionImportanceType QuestionImportanceType { get; set; }
        public List<Answer> AnsweredAnswers { get; set; }
        public Question Question { get; set; }
        public int TimeToAnswer { get; set; }
        public float Correctness { get; set; }
    }
}
