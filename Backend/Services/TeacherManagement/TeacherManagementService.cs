using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.TeacherManagement
{
    public class TeacherManagementService : ITeacherManagementService
    {
        protected readonly DataContext Context;

        public TeacherManagementService(DataContext context)
        {
            Context = context;
        }

        public Teacher GetTeacher(string authName)
        {
            var teacherExists = Context.Teachers
                .FirstOrDefault(x => x.AuthName == authName);
            if (teacherExists == null)
            {
                AddAccount(authName);
            }

            return Context.Teachers
                .Include(c => c.Classes)
                .ThenInclude(s => s.Students)
                .Include(t => t.Topics)
                .ThenInclude(s => s.Scenarios)
                .FirstOrDefault(x => x.AuthName == authName); ;
        }

        private void AddAccount(string authName)
        {
            Context.Teachers.Add(new Teacher
            {
                AuthName = authName,
                Classes = new List<Class>(),
                Topics = new List<Topic>()
            });

            Context.SaveChanges();
        }

        public ICollection<Topic> GetTeacherTopics(string authName)
        {
            var teacher = GetTeacher(authName);
            return teacher.Topics;
        }
        
    }
}