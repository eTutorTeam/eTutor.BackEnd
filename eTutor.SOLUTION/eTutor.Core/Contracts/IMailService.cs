using System;
using System.Threading.Tasks;
using eTutor.Core.Models;

namespace eTutor.Core.Contracts
{
    public interface IMailService
    {
        Task<IOperationResult<int>> SendEmailToRegisteredUser(User user);

        Task<IOperationResult<int>> SendPasswordResetEmail(User user, string token);
    } 
}
