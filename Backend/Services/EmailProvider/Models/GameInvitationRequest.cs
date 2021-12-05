namespace Backend.Services.EmailProvider.Models
{
    public class GameInvitationRequest
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public string Topic { get; set; }
        public string Url { get; set; }
        public string Scenario { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
