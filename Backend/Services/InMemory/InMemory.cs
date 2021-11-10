using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services.InMemory
{
    public class InMemory: IInMemory
    {
        private readonly DataContext _context;

        public InMemory(DataContext context)
        {
            _context = context;
        }

        public int AddScenario(Scenario scenario)
        {
            _context.Scenarios.Add(scenario);
            return _context.SaveChanges();
        }

        public int AddTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            return _context.SaveChanges();
        }
        public List<Scenario> GetScenario()
        {
            return _context.Scenarios.ToList();
        }

        public List<Topic> GetTopic()
        {
            return _context.Topics.ToList();
        }
    }
}
