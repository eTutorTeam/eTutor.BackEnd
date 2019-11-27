using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using eTutor.Core.Models.Configuration;

namespace eTutor.MailService
{
    public sealed class SMTPMailService : IMailService
    {
        private readonly SMTPConfiguration _smtpConfiguration;

        public SMTPMailService(SMTPConfiguration smtpConfiguration)
        {
            _smtpConfiguration = smtpConfiguration;
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

            return SendEmail(parentEmail, "eTutor Student Registration", message);
        }

        private SmtpClient BuildSmtpClient()
        {
            var client = new SmtpClient(_smtpConfiguration.Server, _smtpConfiguration.Port);
            client.Credentials = new NetworkCredential(_smtpConfiguration.User, _smtpConfiguration.Password);
            return client;
        }

        private Task SendEmail(string reciepents, string subject, string content)
        {
            var client = BuildSmtpClient();
            
            var message = new MailMessage(_smtpConfiguration.User, reciepents);
            message.Subject = subject;
            message.Body = content;
            message.IsBodyHtml = true;

            return client.SendMailAsync(message);
        }
    }
}
