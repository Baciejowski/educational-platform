using System.Collections.Generic;
using Backend.Controllers.APIs;
using Backend.Models;

namespace Backend.Services.ClassManagement
{
    public interface IClassManagementService
    {
        IEnumerable<Class> GetClassList();
        void SendGameInvitationToStudents(GameViewModel gameViewModel);
        Teacher GetTeacherByAuthName(string authName);
    }
}