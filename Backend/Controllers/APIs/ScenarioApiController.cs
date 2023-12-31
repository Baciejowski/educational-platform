﻿using System.Collections.Generic;
using Backend.Controllers.APIs.Models;
using Backend.Models;
using Backend.Services.ScenarioManagement;
using Backend.Services.TeacherManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/create-scenario")]
    public class ScenarioApiController : Controller
    {
        private readonly IScenarioManagementService _scenarioManagementService;
        private readonly ITeacherManagementService _teacherManagementService;
        private readonly DataContext _context;

        public ScenarioApiController(IScenarioManagementService scenarioManagementService, ITeacherManagementService teacherManagementService, DataContext context)
        {
            _scenarioManagementService = scenarioManagementService;
            _teacherManagementService = teacherManagementService;
            _context = context;
        }

        
        [HttpGet]
        [Authorize]
        public IEnumerable<Topic> Get()
        {
            var currentUser = HttpContext.User;
            _context.Scenarios.Include(q => q.Questions).ThenInclude(a => a.ABCDAnswers); //TO DO refactoryzacja

            var result=  _teacherManagementService.GetTeacherTopics(currentUser.Identity.Name);
            return result;
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult Post(ScenarioViewModel payload)
        {
            var currentUser = HttpContext.User;
            var teacher = _teacherManagementService.GetTeacher(currentUser.Identity.Name);
            int result = _scenarioManagementService.CreateScenarioFromForm(payload, teacher);
            if (result > 0) return StatusCode(200, result);
            return BadRequest();
        }
    }
}
