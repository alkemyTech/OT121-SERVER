using Microsoft.Extensions.Options;
using OngProject.Core.Entities;
using OngProject.Core.Interfaces.IServices;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class MailService : IMailService
    {
        #region Object and Constructor
        private readonly Entities.MailSettings _mailSettings;
        private readonly MailConstants _mailConstants;
        public MailService(IOptions<Entities.MailSettings> mailSettings, IOptions<MailConstants> mailConstants)
        {
            _mailSettings = mailSettings.Value;
            _mailConstants = mailConstants.Value;
        }
        #endregion

        #region Send Email with parameter EmailAddress, subject and body
        public async Task SendEmailAsync(string ToEmail, string body, string subject)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Send Email with Template
        public async Task SendEmailWithTemplate(string ToEmail, string mailTitle, string mailBody, string mailContact)
        {
            string template = File.ReadAllText(_mailConstants.PathTemplate);
            template = template.Replace(_mailConstants.ReplaceMailTitle, mailTitle);
            template = template.Replace(_mailConstants.ReplaceMailBody, mailBody);
            template = template.Replace(_mailConstants.ReplaceMailContact, mailContact);
            await SendEmailAsync(ToEmail, template, mailTitle);
        }
        #endregion

        #region Send Email when user register
        public async Task SendEmailRegisteredUser(string ToEmail, string fullname)
        {
            string mailBody = _mailConstants.WelcomeMailBody + fullname;
            await SendEmailWithTemplate(ToEmail, _mailConstants.TitleMailConfirm, mailBody, _mailConstants.MailONG);
        }
        #endregion

        #region Send Email when someone contacts by Web 
        public async Task SendEmailRegisteredContact(string ToEmail, string fullname)
        {
            string mailBody = fullname + _mailConstants.ReplyToContact;
            await SendEmailWithTemplate(ToEmail, _mailConstants.TitleMailContact, mailBody, _mailConstants.MailONG);
        }
        #endregion
    }
}
