using Microsoft.Extensions.Options;
using OngProject.Core.Entities;
using OngProject.Core.Interfaces.IServices;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class MailService : IMailService
    {
        #region Object and Constructor
        private readonly Entities.MailSettings _mailSettings;
        public MailService(IOptions<Entities.MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        #endregion

        #region Send Email with parameter EmailAddress, subject and body
        public async Task SendEmailAsync(string ToEmail, string body, string subject)
        {
            var client = new SendGridClient(_mailSettings.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_mailSettings.SenderMail, _mailSettings.SenderName),
                Subject = subject,
                HtmlContent = body
            };
            msg.AddTo(new EmailAddress(ToEmail));

            // disable tracking settings
            // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);

            await client.SendEmailAsync(msg);
        }
        #endregion
    }
}
