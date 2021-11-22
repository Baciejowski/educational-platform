using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Backend.Controllers.Api
{
    [ApiController]
    [Route("api/students")]
    public class StudentsApiController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<StudentsApiController> _logger;

        public StudentsApiController(DataContext dataContext, ILogger<StudentsApiController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;

        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Student student, [Required] int classID)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Class group = _dataContext.Classes.Include(c => c.Teacher).FirstOrDefault(c => c.ClassID==classID);
            if (group == null) return NotFound();
            if (!teacher.Equals(group.Teacher)) return Unauthorized();

            student.Class = group;

            _dataContext.Add(student);

            try
            {
                _dataContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(400, e.Message + " -> " + e.InnerException.Message);
            }

            return StatusCode(201, student.StudentID);

        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Student student, [Required] int classID)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Class group = _dataContext.Classes.Include(c => c.Teacher).FirstOrDefault(c => c.ClassID == classID);
            if (group == null) return NotFound();
            if (!teacher.Equals(group.Teacher)) return Unauthorized();

            Student original = _dataContext.Students.Include(s => s.Class).ThenInclude(c => c.Teacher).FirstOrDefault(s => s.StudentID == student.StudentID);
            if (original== null) return NotFound();
            if (!teacher.Equals(original.Class.Teacher)) return Unauthorized();

            if (!original.Class.Equals(group)) original.Class = group;
            if (!string.Equals(original.FirstName, student.FirstName)) original.FirstName = student.FirstName;
            if (!string.Equals(original.LastName, student.LastName)) original.LastName = student.LastName;
            if (!string.Equals(original.Email, student.Email)) original.Email = student.Email;

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
        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Student student = _dataContext.Students.FirstOrDefault(s => s.StudentID == id);
            if (student == null) return NotFound();

            _dataContext.Entry(student).Reference(s => s.Class).Load();
            _dataContext.Entry(student.Class).Reference(c => c.Teacher).Load();
            if (!teacher.Equals(student.Class.Teacher)) return Unauthorized();

            _dataContext.Remove(student);

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
