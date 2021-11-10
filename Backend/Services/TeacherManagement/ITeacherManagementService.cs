using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services.TeacherManagement
{
    public interface ITeacherManagementService
    {
        Teacher GetTeacher(string authName);
        ICollection<Topic> GetTeacherTopics(string authName);
    }
}
