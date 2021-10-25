using System.Collections.Generic;
using Backend.Models;
using Backend.Services.ClassManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        public IEnumerable<Class> Get()
        {
            var currentUser = HttpContext.User;

            return _classManagementService.GetClassList();
        }
        
        [HttpPost]
        [Authorize]
        public OkResult Post(GameViewModel payload)
        {
            var currentUser = HttpContext.User;
            _classManagementService.SendGameInvitationToStudents(payload);

            return Ok();
        }
    }
}