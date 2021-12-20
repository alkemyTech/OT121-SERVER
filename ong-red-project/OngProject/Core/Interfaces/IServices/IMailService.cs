using System;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IMailService
    {
        Task SendEmailAsync(string ToEmail, string body, string subject);
        Task SendEmailWithTemplate(string ToEmail, string mailTitle, string mailBody, string mailContact);
        Task SendEmailRegisteredUser(string ToEmail, string fullname);
        Task SendEmailRegisteredContact(string ToEmail, string fullname);
    }
}
