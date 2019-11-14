using System;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace eTutor.SendGridMail
{
    public class MailService : IMailService
    {

        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;

             _apiKey = _configuration.GetSection("SendGrid")["ApiKey"];
            
            
        }


        public async Task<IOperationResult<int>> SendEmailToRegisteredUser(int userId)
        {
            var sendGridClient = new SendGridClient(_apiKey);
            var from = new EmailAddress("no_reply@etutor.com", "Etutor");
            var to = new EmailAddress("juandanielozuna2@gmail.com", "Juan Daniel Ozuna");
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, "d-307bb4ffcd3747d5bb22f5cf096cc689", new { name = "Juan Daniel Ozuna" });
            var response = await sendGridClient.SendEmailAsync(msg);

            if (response.StatusCode >= System.Net.HttpStatusCode.BadRequest)
            {
                return BasicOperationResult<int>.Fail("Error sending mail");
            }

            return BasicOperationResult<int>.Ok();
        }

        public async Task<IOperationResult<int>> SendPasswordResetEmail(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
