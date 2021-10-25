using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Controllers.APIs;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services.ClassManagement
{
    public interface IClassManagementService
    {
        IEnumerable<Class> GetClassList();
        void SendGameInvitationToStudents(GameViewModel gameViewModel);
    }
}
