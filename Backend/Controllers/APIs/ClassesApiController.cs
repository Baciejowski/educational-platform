using Backend.Controllers.APIs.Models;
using Backend.Models;
using Backend.Services.ClassManagement;
using Backend.Services.TeacherManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/classes")]
    public class ClassesApiController : Controller
    {
        private readonly IClassManagementService _classManagementService;
        private readonly DataContext _dataContext;

        public ClassesApiController(IClassManagementService classManagementService, DataContext dataContext)
        {
            _classManagementService = classManagementService;
            _dataContext = dataContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();
            _dataContext.Entry(teacher).Collection(t => t.Classes).Load();
            foreach (Class group in teacher.Classes)
            {
                _dataContext.Entry(group).Collection(c => c.Students).Load();
            }

            return new ObjectResult(teacher.Classes);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Class group)
        {
            if (group.FriendlyName.Length == 0 || group.FriendlyName.Length > 25) return BadRequest();
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();
            group.Teacher = teacher;
            _dataContext.Add(group);
            try
            {
                _dataContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(400, e.Message + " -> " + e.InnerException.Message);
            }
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Class group)
        {
            if (group.FriendlyName.Length == 0 || group.FriendlyName.Length > 25) return BadRequest();
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();
            Class original = _dataContext.Classes.FirstOrDefault(c => c.ClassID == group.ClassID);
            if (original == null) return NotFound();
            _dataContext.Entry(original).Reference(o => o.Teacher).Load();
            if (!teacher.Equals(original.Teacher)) return Unauthorized();

            if (!string.Equals(original.FriendlyName, group.FriendlyName)) original.FriendlyName = group.FriendlyName;

            _dataContext.Update(original);
            try
            {
                _dataContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(400, e.Message + " -> " + e.InnerException.Message);
            }
            return Ok();
        }

        [Route("api/classes/invitations")]
        [HttpPost]
        [Authorize]
        public OkResult Post(ClassesGameInvitationViewModel payload)
        {
            var currentUser = HttpContext.User;
            _classManagementService.SendGameInvitationToStudents(payload, currentUser.Identity.Name);

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Class group = _dataContext.Classes.Include(c => c.Teacher).FirstOrDefault(c => c.ClassID == id);
            if (group == null) return NotFound();

            _dataContext.Entry(group).Reference(topic => topic.Teacher).Load();
            if (!group.Teacher.Equals(teacher)) return Unauthorized();

            _dataContext.Remove(group);
            try
            {
                _dataContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(400, e.Message + " -> " + e.InnerException.Message);
            }
            return Ok();
        }
    }
}