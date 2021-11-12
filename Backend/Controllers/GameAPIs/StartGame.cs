using Backend.Analysis_module;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Gameplay;

namespace Backend.Controllers.GameAPIs
{
    [ApiController]
    public class StartGame : Controller
    {
        private readonly ILogger<StartGame> _logger;
        private readonly IAnalysisModuleService _analysisModuleService;

        public StartGame(ILogger<StartGame> logger, IAnalysisModuleService analysisModuleService)
        {
            _logger = logger;
            _analysisModuleService = analysisModuleService;
        }

        [HttpPost]
        [Route("api-game/start-game")]
        public IActionResult PostStartGame()
        {
            var msg = ProtoReader.Convert<StartGameRequest>(Request);

            // Malformed requests
            if (msg == null) return BadRequest();
            if (msg.Code.Length == 0 || msg.Email.Length == 0) return BadRequest();

            var response =
                _analysisModuleService.StartNewSession(msg);

            //Proper requests
            return ProtoResponse.FromMsg(response);
        }

        [HttpPost]
        [Route("api-game/next-question")]
        public IActionResult PostNextQuestion()
        {
            var msg = ProtoReader.Convert<QuestionRequest>(Request);

            // Malformed requests
            if (msg == null) return BadRequest();
            if (msg.SessionCode.Length == 0) return BadRequest();

            var response =
                _analysisModuleService.PrepareNextQuestion(msg);

            //Proper requests
            return ProtoResponse.FromMsg(response);
        }

        [HttpPost]
        [Route("api-game/answer")]
        public IActionResult PostAnswer()
        {
            var msg = ProtoReader.Convert<StudentAnswerRequest>(Request);

            // Malformed requests
            if (msg == null) return BadRequest();
            if (msg.SessionCode.Length == 0) return BadRequest();

            var response =
                _analysisModuleService.UpdateStudentsAnswers(msg);

            //Proper requests
            return ProtoResponse.FromMsg(response);
        }

        [HttpPost]
        [Route("api-game/endgame")]
        public IActionResult PostEndgame()
        {
            var msg = ProtoReader.Convert<EndGameRequest>(Request);

            // Malformed requests
            if (msg == null) return BadRequest();
            if (msg.SessionCode.Length == 0) return BadRequest();

            var response =
                _analysisModuleService.EndGame(msg);

            //Proper requests
            return ProtoResponse.FromMsg(response);
        }
    }
}