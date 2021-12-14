﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class ClassesController : Controller
    {
        [EnableCors]
        [HttpGet]
        public IEnumerable<Class> Get()
        {
            var currentUser = HttpContext.User;
            var resultClassesList = new Class[] {
                new Class {
                    Id = "1",
                    Students = new []{
                        new Student{
                            Id="1",
                            Email="ola.swierczek111@gmail.com"
                        },

                        new Student{
                            Id="1",
                            Email="ola.swierczek111@gmail.com"
                        }
                    }
                }
            };

            return resultClassesList;
        }

        public class Class
        {
            public string Id { get; set; }
            public Student[] Students { get; set; }
        }
        public class Student
        {
            public string Id { get; set; }
            public string Email { get; set; }
        }
    }
}