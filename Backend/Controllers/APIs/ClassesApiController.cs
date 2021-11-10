using System.Collections.Generic;
using Backend.Models;
using Backend.Services.ClassManagement;
using Backend.Services.TeacherManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/classes")]
    public class ClassesApiController : Controller
    {
        private readonly IClassManagementService _classManagementService;
        private readonly ITeacherManagementService _teacherManagementService;

        public ClassesApiController(IClassManagementService classManagementService, ITeacherManagementService teacherManagementService)
        {
            _classManagementService = classManagementService;
            _teacherManagementService = teacherManagementService;
        }
        
        [HttpGet]
        [Authorize]
        public Teacher Get()
        {
            var currentUser = HttpContext.User;
            return _teacherManagementService.GetTeacher(currentUser.Identity.Name);
        }
        
        [HttpPost]
        [Authorize]
        public OkResult Post(ClassesGameInvitationViewModel payload)
        {
            var currentUser = HttpContext.User;
            _classManagementService.SendGameInvitationToStudents(payload, currentUser.Identity.Name);

            return Ok();
        }
    }
}