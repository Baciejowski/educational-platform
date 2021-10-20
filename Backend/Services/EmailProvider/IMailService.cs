using System.Threading.Tasks;

namespace Backend.Services.EmailProvider
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
