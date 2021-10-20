using System.Threading.Tasks;
using Backend.Services.EmailProvider.Models;

namespace Backend.Services.EmailProvider
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendGameInvitationRequestAsync(GameInvitationRequest request);
    }
}
