using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Question
    {
        public enum TypeEnum : byte
        {
            BOOLEAN = 0,
            ABCD = 1,
            OPEN = 2
        }
        public int QuestionID { get; set; }
        public byte Difficulty { get; set; }
        public string Content { get; set; }
        public string Hint { get; set; }
        public TypeEnum QuestionType { get; set; }
        public bool? BooleanAnswer { get; set; }
        public bool IsImportant { get; set; }
        public bool IsObligatory { get; set; }
        public virtual ICollection<Answer> ABCDAnswers { get; set; }

        [JsonIgnore]
        public virtual ICollection<Scenario> Scenarios { get; set; }
    }
}
