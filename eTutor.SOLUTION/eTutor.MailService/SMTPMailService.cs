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
using Microsoft.Extensions.Configuration;
using NETCore.MailKit.Core;

namespace eTutor.MailService
{
    public sealed class SMTPMailService : IMailService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly string _parentLink;

        public SMTPMailService(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
            _baseUrl = _configuration["BaseSiteUrl"];
            IConfigurationSection links = _configuration.GetSection("EmailLinks");
            _parentLink = links["ParentLink"];
        }

        public Task SendEmailToRegisteredUser(User user)
        {
            string message = $"<h1>Hola {user.FullName}</h1>\r\n\r\n" +
                             $"<p>Su cuenta ha sido creada exitosamente,\r\nEstaremos validando su ingreso como tutor en unos momentos</p>";

            return SendEmail($"{user.Email}, juandanielozuna2@gmail.com", "Su solicitud para su Cuenta ha sido Tomada", message);
        }

        public Task SendPasswordResetEmail(User user, string token)
        {
            string message = $"User reset password for {user.Email}";

            return SendEmail(user.Email, "eTutor Password Reset", message);
        }

        public Task SendEmailToCreatedStudentUser(User user)
        {
            string message = $"<h1>Hola {user.FullName}</h1>\r\n\r\n" +
                             $"<p>Su cuenta ha sido creada exitosamente,\r\nle hemos enviado un mensaje a al correo electronico del padre\r\nsuministrado," +
                             $" para que el mismo pueda proceder a activar su cuenta </p>\r\n\r\n<p>Le dejaremos saber cuando su cuenta haya sido activada, por su Padre</p>";

            return SendEmail($"{user.Email}, juandanielozuna2@gmail.com", $"Tu cuenta ha sido creada: {user.FullName}",
                new EmailModel {HtmlContent = message});
        }

        public Task SendEmailToParentToCreateAccountAndValidateStudent(User user, string parentEmail)
        {
            string message = $"<h1>Buenas Sr/Sra</h1>\r\n<p>Le queremos comunicar que su hijo/a {user.FullName}\r\n   " +
                             $" a creado una cuenta en nuestro sistema con el correo\r\n    electronico {user.Email}\r\n</p>\r\n\r\n" +
                             "<p>Para que su hijo pueda proceder con el registro,\r\n   " +
                             " debe de proceder a registrarse como usuario y validar a su \r\n    hijo presionando el boton de abajo\r\n</p>";


            string link = string.Format(_parentLink, user.Id, parentEmail);

            var emailModel = new EmailModel
            {
                BtnDisplay = "block",
                BtnText = "Continuar Registro",
                HtmlContent = message,
                Link = $"{_baseUrl}{link}"
            };

            return SendEmail($"{parentEmail}, juandanielozuna2@gmail.com", "Proceso de Registro Aplicacion eTutor",
                emailModel);
        }

        public Task SendEmailForSuccesfullAcountCreation(User user)
        {
            string message = $"<h1>Buenas {user.FullName}</h1>\r\n<h2>Su cuenta ha sido creada exitosamente</h2>";

            return SendEmail($"{user.Email}, juandanielozuna2@gmail.com", "Su cuenta ha sido activada", message);
        }

        public Task SendEmailStudentActivated(User user)
        {
            string message = $"<h1>Buenas {user.FullName}</h1>\r\n<h2>Su cuenta ha sido activada exitosamente</h2>" +
                             "\r\n\r\n<p>Ya puede proceder a ingresar a la aplicacion\r\n</p>\r\n<br>";

            return SendEmail($"{user.Email}, juandanielozuna2@gmail.com", "Su cuenta ha sido activada", message);
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
