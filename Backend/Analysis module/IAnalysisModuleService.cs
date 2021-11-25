using Gameplay;

namespace Backend.Analysis_module
{
    public interface IAnalysisModuleService
    {
        StartGameResponse StartNewSession(StartGameRequest request, DataContext Context);
        QuestionResponse PrepareNextQuestion(QuestionRequest request);
        Empty UpdateStudentsAnswers(StudentAnswerRequest request);
        Empty EndGame(EndGameRequest request);
    }
}
