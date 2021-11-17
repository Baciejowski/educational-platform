using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/topics")]
    public class TopicApiController : Controller
    {
        private readonly DataContext _dataContext;

        public TopicApiController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(bool includeScenarios)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();
            _dataContext.Entry(teacher).Collection(t => t.Topics).Load();
            if (includeScenarios)
                foreach (Topic topic in teacher.Topics)
                    _dataContext.Entry(topic).Collection(t => t.Scenarios).Load();
            return new ObjectResult(teacher.Topics);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Topic topic)
        {
            if (topic.TopicName.Length == 0 || topic.TopicName.Length > 30) return BadRequest();
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();
            topic.Teacher = teacher;
            _dataContext.Add(topic);
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
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Topic topic = _dataContext.Topics.FirstOrDefault(t => t.TopicID == id);
            if (topic == null) return NotFound();

            _dataContext.Entry(topic).Reference(topic => topic.Teacher).Load();
            if (!topic.Teacher.Equals(teacher)) return Unauthorized();
            if (_dataContext.Entry(topic).Collection(t => t.Scenarios).Query().Count() > 0) return Conflict();

            _dataContext.Remove(topic);
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
