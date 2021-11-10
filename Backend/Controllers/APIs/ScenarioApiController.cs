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
    [Route("api/create-scenario")]
    public class ScenarioApiController : Controller
    {
        private readonly IScenarioManagementService _scenarioManagementService;
        private readonly ITeacherManagementService _teacherManagementService;

        public ScenarioApiController(IScenarioManagementService scenarioManagementService, ITeacherManagementService teacherManagementService)
        {
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
        
        [HttpPost]
        [Authorize]
        public OkResult Post(ScenarioViewModel payload)
        {
            var currentUser = HttpContext.User;
            var teacher = _teacherManagementService.GetTeacher(currentUser.Identity.Name);
            _scenarioManagementService.CreateScenarioFromForm(payload, teacher);
        
            return Ok();
        }
    }
}
