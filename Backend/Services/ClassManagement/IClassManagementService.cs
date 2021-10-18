using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services.ClassManagement
{
    public interface IClassManagementService
    {
        IEnumerable<Class> GetMockedClassList();
    }
}
