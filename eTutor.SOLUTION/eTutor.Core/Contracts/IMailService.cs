using System;
using System.Threading.Tasks;
using eTutor.Core.Models;

namespace eTutor.Core.Contracts
{
    public interface IMailService
    {
        Task SendEmailToRegisteredUser(User user);
        Task SendPasswordResetEmail(User user, string token);
        Task SendEmailToCreatedStudentUser(User user);
        Task SendEmailToParentToCreateAccountAndValidateStudent(User user, string parentEmail);
        Task SendEmailForSuccesfullAcountCreation(User user);
        Task SendEmailStudentActivated();
    } 
}
