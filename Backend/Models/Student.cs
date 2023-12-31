﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public virtual Class Class { get; set; }

        [JsonIgnore]
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
