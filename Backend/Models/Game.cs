using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Game
    {
        public int GameID { get; set; }
        // TODO: Add ratings
        public virtual Scenario Scenario {get; set;}
        public virtual Student Student { get; set; }
    }
}
