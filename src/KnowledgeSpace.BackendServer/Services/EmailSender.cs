using Microsoft.AspNetCore.Identity.UI.Services;

namespace KnowledgeSpace.BackendServer.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
