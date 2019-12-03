using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using eTutor.Core.Models.Configuration;
using NETCore.MailKit.Core;

namespace eTutor.MailService
{
    public sealed class SMTPMailService : IMailService
    {
        private readonly IEmailService _emailService;

        public SMTPMailService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task SendEmailToRegisteredUser(User user)
        {
            string message = $"User registration completed for {user.Email}";

            return SendEmail(user.Email, "eTutor Registration", message);
        }

        public Task SendPasswordResetEmail(User user, string token)
        {
            string message = $"User reset password for {user.Email}";

            return SendEmail(user.Email, "eTutor Password Reset", message);
        }

        public Task SendEmailToValidateParent(string parentEmail)
        {
            string message = $"Su hijo se ha registrado recientemente en el sistema {parentEmail}";

            return _emailService.SendAsync(parentEmail, "Padre email", message, true);
        }

        public Task SendEmailToCreatedStudentUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailToParentToCreateAccountAndValidateStudent(User user, string parentEmail)
        {
            throw new NotImplementedException();
        }

        private async Task<string> ReadEmailTemplate(string fileName)
        {
            var directory = Directory.GetCurrentDirectory();
            return "";
        }

        private Task SendEmail(string reciepents, string subject, string content)
        {
            var message = new MailMessage("", reciepents);
            message.Subject = subject;
            message.Body = content;
            message.IsBodyHtml = true;

            return _emailService.SendAsync(reciepents, message.Subject, message.Body, message.IsBodyHtml);
        }
    }
}
