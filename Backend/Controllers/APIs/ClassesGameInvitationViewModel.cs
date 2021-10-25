using System;

namespace Backend.Controllers.APIs
{
    public class ClassesGameInvitationViewModel
    {
        public int ClassId { get; set; }
        public int TopicId { get; set; }
        public int ScenarioId { get; set; }
        public int TeacherId { get; set; }
        public DateTime StartGame { get; set; }
        public DateTime EndGame { get; set; }
    }
}