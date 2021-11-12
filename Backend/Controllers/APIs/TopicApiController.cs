using System.Collections.Generic;
using Backend.Models;
using Backend.Services.ClassManagement;
using Backend.Services.ScenarioManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/topics")]
    public class TopicApiController : Controller
    {
        private readonly IClassManagementService _classManagementService;
        private readonly IScenarioManagementService _scenarioManagementService;

        public TopicApiController(IClassManagementService classManagementService, IScenarioManagementService scenarioManagementService)
        {
            _classManagementService = classManagementService;
            _scenarioManagementService = scenarioManagementService;
        }

        
        [HttpGet]
        [Authorize]
        public IEnumerable<Topic> Get()
        {
            var currentUser = HttpContext.User;
        
            return _classManagementService.GetTeacherTopics(currentUser.Identity.Name);
        }
    }
}
