using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Scenario
    {
        public int ScenarioID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual ICollection<Game> Games { get; set; }

    }
}
