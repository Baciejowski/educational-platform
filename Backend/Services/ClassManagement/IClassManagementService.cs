using System.Collections.Generic;
using Backend.Controllers.APIs;
using Backend.Models;

namespace Backend.Services.ClassManagement
{
    public interface IClassManagementService
    {
        IEnumerable<Class> GetClassList();
        void SendGameInvitationToStudents(ClassesGameInvitationViewModel classesGameInvitationViewModel);
        Teacher GetTeacherByAuthName(string authName);
        IEnumerable<Topic> GetTeacherTopics(string? authName);
    }
}