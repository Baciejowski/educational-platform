using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Gameplay;

namespace Backend.Controllers.GameAPIs
{
    [ApiController]
    [Route("api-game/start-game")]
    public class StartGame : Controller
    {

        private readonly ILogger<StartGame> _logger;
        public StartGame(ILogger<StartGame> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post()
        {
            var msg = ProtoReader.Convert<StartGameRequest>(Request);

            // Malformed requests
            if (msg == null) return BadRequest();
            if (msg.Code.Length == 0 || msg.Email.Length == 0) return BadRequest();

            //Proper requests
            return ProtoResponse.FromMsg(new StartGameResponse
            {
                Error = true,
                ErrorMsg = "Not implemented"
            });
        }
    }

}
