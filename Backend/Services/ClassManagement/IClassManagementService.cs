using System;
using System.Collections.Generic;
using Backend.Controllers.APIs;
using Backend.Controllers.APIs.Models;
using Backend.Models;

namespace Backend.Services.ClassManagement
{
    public interface IClassManagementService
    {
        void SendGameInvitationToStudents(ClassesGameInvitationViewModel classesGameInvitationViewModel, string authName);
    }
}