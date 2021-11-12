using System.Collections.Generic;
using Backend.Models;

namespace Backend.Services.TeacherManagement
{
    public interface ITeacherManagementService
    {
        Teacher GetTeacher(string authName);
        ICollection<Topic> GetTeacherTopics(string authName);
    }
}
