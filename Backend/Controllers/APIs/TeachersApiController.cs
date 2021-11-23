using Backend.Controllers.APIs.Models;
using Backend.Models;
using Backend.Services.ClassManagement;
using Backend.Services.TeacherManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/teachers")]
    public class TeachersApiController : Controller
    {
        private readonly ITeacherManagementService _teacherManagementService;

        public TeachersApiController(ITeacherManagementService teacherManagementService)
        {
            _teacherManagementService = teacherManagementService;
        }

        [HttpGet]
        [Authorize]
        public Teacher Get()
        {
            var currentUser = HttpContext.User;
            return _teacherManagementService.GetTeacher(currentUser.Identity.Name);
        }
    }
}