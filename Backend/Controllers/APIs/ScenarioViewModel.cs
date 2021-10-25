using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers.APIs
{
    public class ScenarioViewModel
    {
        public string Name { get; set; }
        public dynamic Questions { get; set; }
        public string Topic { get; set; }
    }
}
