using System.Threading.Tasks;
using Apae.Data;
using Apae.Emails;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Apae.Services
{
    public interface IEmailSender
    {
        Task SendInvitationAsync(User user, string from, string resetLink);

        Task SendResetPasswordAsync(User user, string resetLink);
    }

    public partial class EmailSender : IEmailSender
    {
        private readonly EmailAddress SystemAddress = new EmailAddress("no-reply@tangram.social", "Tangram Social");

        private readonly SendGridClient _client;

        public EmailSender(IConfiguration configuration)
        {
            _client = new SendGridClient(configuration["SendGrid:ApiKey"]);
        }

        public Task SendInvitationAsync(User user, string from, string resetLink)
        {
            var data = new InvitationTemplateData(user)
            {
                InvitedBy = from,
                ResetPasswordLink = resetLink

            };

            return SendAsync(user, "d-3a00bacb9c8c481aacd256000c680e74", data);
        }

        public Task SendResetPasswordAsync(User user, string resetLink)
        {
            var data = new ResetPasswordTemplateData(user)
            {
                ResetPasswordLink = resetLink
            };

            return SendAsync(user, "d-afeadd1ef7c143a88f838fd5797338d5", data);
            
        }

        private async Task SendAsync<T>(User user, string templateId, T templateData)
        {
            var msg = new SendGridMessage();

            msg.SetFrom(SystemAddress);
            msg.AddTo(user.Email, user.FullName);

            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);

            await _client.SendEmailAsync(msg);
        }
    }




}
