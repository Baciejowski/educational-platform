﻿using System.Collections.Generic;
using Backend.Models;
using Backend.Services.ClassManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/classes")]
    public class ClassesApiController : Controller
    {
        private readonly IClassManagementService _classManagementService;

        public ClassesApiController(IClassManagementService classManagementService)
        {
            _classManagementService = classManagementService;
        }
        
        [HttpGet]
        [Authorize]
        public Teacher Get()
        {
            var currentUser = HttpContext.User;

            return _classManagementService.GetTeacherData(currentUser.Identity.Name);
        }
        
        [HttpPost]
        [Authorize]
        public OkResult Post(ClassesGameInvitationViewModel payload)
        {
            var currentUser = HttpContext.User;
            _classManagementService.SendGameInvitationToStudents(payload);

            return Ok();
        }
    }
}