using Backend.Analysis_module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Gameplay;

namespace Backend.Controllers.GameAPIs
{
    [ApiController]
    public class StartGame : Controller
    {
        private readonly DataContext _context;
        private readonly ILogger<StartGame> _logger;
        private readonly IAnalysisModuleService _analysisModuleService;

        public StartGame(ILogger<StartGame> logger, IAnalysisModuleService analysisModuleService, DataContext context)
        {
            _logger = logger;
            _analysisModuleService = analysisModuleService;
            _context = context;
        }

        [HttpPost]
        [Route("api-game/start-game")]
        public IActionResult PostStartGame()
        {
            var msg = ProtoReader.Convert<StartGameRequest>(Request);
            var response = _analysisModuleService.StartNewSession(msg, _context);
            return ProtoResponse.FromMsg(response);
        }

        [HttpPost]
        [Route("api-game/next-question")]
        public IActionResult PostNextQuestion()
        {
            var msg = ProtoReader.Convert<QuestionRequest>(Request);
            try
            {
                var response = _analysisModuleService.PrepareNextQuestion(msg);
                return ProtoResponse.FromMsg(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api-game/answer")]
        public IActionResult PostAnswer()
        {
            var msg = ProtoReader.Convert<StudentAnswerRequest>(Request);
            try
            {
                var response = _analysisModuleService.UpdateStudentsAnswers(msg);
                return ProtoResponse.FromMsg(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api-game/endgame")]
        public IActionResult PostEndgame()
        {
            var msg = ProtoReader.Convert<EndGameRequest>(Request);
            // try
            // {
            var response = _analysisModuleService.EndGame(msg, _context);
            return ProtoResponse.FromMsg(response);
            // }
            // catch
            // {
            //     return BadRequest();
            // }
        }
    }
}