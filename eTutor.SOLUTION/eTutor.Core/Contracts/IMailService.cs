using System;
using System.Threading.Tasks;

namespace eTutor.Core.Contracts
{
    public interface IMailService
    {
        Task<IOperationResult<int>> SendEmailToRegisteredUser(int userId);

        Task<IOperationResult<int>> SendPasswordResetEmail(int userId);
    } 
}
