using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay;

namespace Backend.Analysis_module
{
    public interface IAnalysisModuleService
    {
        StartGameResponse StartNewSession(StartGameRequest request);

        QuestionResponse PrepareNextQuestion(QuestionRequest request);
        Empty UpdateStudentsAnswers(StudentAnswerRequest request);
        Empty EndGame(EndGameRequest request);
    }
}
