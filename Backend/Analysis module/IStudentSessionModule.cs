using System.Collections.Generic;
using Backend.Analysis_module.Models;
using Backend.Models;
using Gameplay;

namespace Backend.Analysis_module
{
    public interface IStudentSessionModule
    {
        StudentSessionData StudentSessionData { get; set; }
        void EndGame(EndGameRequest request);
        void SaveAnswerResponse(AnsweredQuestionModel studentResponseAdapter);
        QuestionResponse.Types.QuestionReward CalculateReward();
        Question GetQuestion(QuestionImportanceType requestQuestionType);
        IEnumerable<int> GetQuestionsAmount();
    }
}