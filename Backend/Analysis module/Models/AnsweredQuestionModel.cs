using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Google.Protobuf.Collections;

namespace Backend.Analysis_module.Models
{
    public class AnsweredQuestionModel
    {
        public QuestionImportanceType QuestionImportanceType { get; set; }
        public RepeatedField<uint> AnswersId { get; set; }
        public Question Question { get; set; }
        public uint TimeToAnswer { get; set; }
    }
}
