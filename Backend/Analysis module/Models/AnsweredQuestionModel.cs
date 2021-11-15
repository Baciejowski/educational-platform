﻿using Backend.Models;
using Google.Protobuf.Collections;

namespace Backend.Analysis_module.Models
{
    public class AnsweredQuestionModel
    {
        public QuestionImportanceType QuestionImportanceType { get; set; }
        public RepeatedField<int> AnswersId { get; set; }
        public Question Question { get; set; }
        public int TimeToAnswer { get; set; }
    }
}
