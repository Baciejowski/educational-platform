using System.Collections.Generic;
using Backend.Models;
using Backend.Services.ClassManagement;
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
        [EnableCors]
        [HttpGet]
        public IEnumerable<Class> Get()
        {
            var currentUser = HttpContext.User;

            return _classManagementService.GetMockedClassList();
        }
    }
}