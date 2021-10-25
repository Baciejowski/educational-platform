using System.Collections.Generic;
using Backend.Models;

namespace Backend.Controllers.APIs
{
    public class ScenarioViewModel
    {
        public string Name { get; set; }
        public IEnumerable<IEnumerable<ScenarioQuestionsViewModel>> Questions { get; set; }
        public string Topic { get; set; }
    }

    public class ScenarioQuestionsViewModel
    {
        public IEnumerable<ScenarioQuestionsAnswerViewModel> Answers { get; set; }
        public byte Difficulty { get; set; }
        public string Content { get; set; }
        public Question.TypeEnum QuestionType { get; set; }
        public bool IsImportant { get; set; }
        public bool Obligatory { get; set; }
    }

    public class ScenarioQuestionsAnswerViewModel
    {
        public bool IsCorrect { get; set; }
        public string Value { get; set; }
    }
}