using System.Collections.Generic;
using Backend.Models;
using Backend.Services.ClassManagement;
using Backend.Services.ScenarioManagement;
using Backend.Services.TeacherManagement;
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
        private readonly ITeacherManagementService _teacherManagementService;

        public TopicApiController(IClassManagementService classManagementService, IScenarioManagementService scenarioManagementService, ITeacherManagementService teacherManagementService)
        {
            _classManagementService = classManagementService;
            _scenarioManagementService = scenarioManagementService;
            _teacherManagementService = teacherManagementService;
        }

        
        [HttpGet]
        [Authorize]
        public IEnumerable<Topic> Get()
        {
            var currentUser = HttpContext.User;
        
            return _teacherManagementService.GetTeacherTopics(currentUser.Identity.Name);
        }
    }
}
