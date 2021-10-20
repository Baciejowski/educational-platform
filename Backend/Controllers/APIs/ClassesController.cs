using System.Collections.Generic;
using Backend.Models;
using Backend.Services.ClassManagement;
using Communication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class ClassesController : Controller
    {
        private readonly IClassManagementService _classManagementService;

        public ClassesController(IClassManagementService classManagementService)
        {
            _classManagementService = classManagementService;
        }
        
        [HttpGet]
        public IEnumerable<Class> Get()
        {
            var currentUser = HttpContext.User;

            return _classManagementService.GetClassList();
        }

        [EnableCors]
        [HttpPost]
        public OkResult Post(GameViewModel payload)
        {
            var currentUser = HttpContext.User;
            _classManagementService.SendGameInvitationToStudents(payload);

            return Ok();
        }
    }
}