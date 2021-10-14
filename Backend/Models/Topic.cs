﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Topic
    {
        public int TopicID { get; set; }
        public int TopicName { get; set; }
        public virtual ICollection<Scenario> Scenarios { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
