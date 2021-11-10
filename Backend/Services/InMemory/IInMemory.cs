using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services.InMemory
{
    interface IInMemory
    {
        int AddScenario(Scenario scenario);
        int AddTopic(Topic topic);
    }
}
