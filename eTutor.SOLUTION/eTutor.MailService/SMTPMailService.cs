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

        public Task SendEmailForSuccesfullAcountCreation(User user)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailStudentActivated()
        {
            throw new NotImplementedException();
        }

        private async Task<string> ReadEmailTemplate(string fileName = "generic-email.html")
        {
            var directory = Directory.GetCurrentDirectory();
            var complete = Path.Combine(directory, "Templates", fileName);

            string fileContent = await File.ReadAllTextAsync(complete);
            return fileContent;
        }

        private async Task SendEmail(string reciepents, string subject, EmailModel model)
        {
            string templateContent = await ReadEmailTemplate();

            templateContent = templateContent.Replace("{{html-content}}", model.HtmlContent);
            templateContent = templateContent.Replace("{{btn-text}}", model.BtnText);
            templateContent = templateContent.Replace("{{btn-display}}", model.BtnDisplay);
            templateContent = templateContent.Replace("{{link}}", model.Link);

            await _emailService.SendAsync(reciepents, subject, templateContent, true);
        }

        private async Task SendEmail(string reciepents, string subject, string htmlMessage)
        {
            var model = new EmailModel()
            {
                BtnDisplay = "none",
                BtnText = "",
                HtmlContent = htmlMessage,
                Link = ""
            };

            string templateContent = await ReadEmailTemplate();

            templateContent = templateContent.Replace("{{html-content}}", model.HtmlContent);
            templateContent = templateContent.Replace("{{btn-text}}", model.BtnText);
            templateContent = templateContent.Replace("{{btn-display}}", model.BtnDisplay);
            templateContent = templateContent.Replace("{{link}}", model.Link);

            await _emailService.SendAsync(reciepents, subject, templateContent, true);
        }
    }
}
