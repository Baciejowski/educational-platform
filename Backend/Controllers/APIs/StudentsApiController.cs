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
        public IActionResult Post([FromBody] Student student)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Class group = _dataContext.Classes.FirstOrDefault(c => c.Teacher.Equals(teacher));
            if (group == null) return NotFound();

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
    }
}
