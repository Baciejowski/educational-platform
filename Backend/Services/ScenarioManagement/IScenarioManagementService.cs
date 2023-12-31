﻿using Backend.Controllers.APIs;
using Backend.Controllers.APIs.Models;
using Backend.Models;

namespace Backend.Services.ScenarioManagement
{
    public interface IScenarioManagementService
    {
        int CreateScenarioFromForm(ScenarioViewModel payload, Teacher teacher);
    }
}
