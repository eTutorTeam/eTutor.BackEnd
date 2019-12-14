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
        Task SendEmailToParentToCreateAccountAndValidateStudent(User studentUser, string parentEmail);
        Task SendToValidateEmail(User user);
        Task SendWhenAccountStateToggled(User user);
        Task SendEmailToExistingParentToValidateStudent(User studentUser, User parentUser);
        Task SendEmailForSuccesfullAcountCreation(User user);
        Task SendEmailStudentActivated(User user);
    } 
}
