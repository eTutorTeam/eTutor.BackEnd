using System;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace eTutor.SendGridMail
{
    public class SendGridMailService : IMailService
    {

        private readonly IConfiguration _configuration;
        private readonly string _apiKey;


        public SendGridMailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration.GetSection("SendGrid")["ApiKey"];
        }


        public async Task SendEmailToRegisteredUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task SendPasswordResetEmail(User user, string token)
        {
            var passwordRecoveryUrl = _configuration.GetSection("Settings")["PasswordRecoveryUrl"];
            passwordRecoveryUrl = passwordRecoveryUrl.Replace("{userId}", token);

            var sendGridClient = new SendGridClient(_apiKey);
            var from = new EmailAddress("no_reply@etutor.com", "Etutor");
            var to = new EmailAddress(user.Email, user.FullName);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, "d-307bb4ffcd3747d5bb22f5cf096cc689",
                new { Name = "Juan Daniel Ozuna", ResetLink = "https://google.com" });
            Response response = await sendGridClient.SendEmailAsync(msg);
        }

        public Task SendEmailToValidateParent(string parentEmail)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailToCreatedStudentUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailToParentToCreateAccountAndValidateStudent(User studentUser, string parentEmail)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailToExistingParentToValidateStudent(User studentUser, User parentUser)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailForSuccesfullAcountCreation(User user)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailStudentActivated(User user)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailStudentActivated()
        {
            throw new NotImplementedException();
        }
    }
}
