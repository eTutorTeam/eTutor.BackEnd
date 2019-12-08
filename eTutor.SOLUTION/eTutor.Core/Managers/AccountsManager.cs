using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Core.Managers
{
    public class AccountsManager
    {
        private readonly IChangePasswordRepository _changePasswordRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        public AccountsManager(IChangePasswordRepository changePasswordRepository, IUserRepository userRepository, IMailService mailService)
        {
            _changePasswordRepository = changePasswordRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public async Task<IOperationResult<ChangePassword>> GenerateChangePasswordRequest(int userId)
        {
            var user = await _userRepository.Find(u => u.Id == userId);

            if (user == null)
            {
                return BasicOperationResult<ChangePassword>.Fail("El usuario no fue encontrado");
            }

            ChangePassword changePassword = new ChangePassword() {ChangeRequestId = Guid.NewGuid(), UserId = userId};

            _changePasswordRepository.Create(changePassword);

            await _changePasswordRepository.Save();
            await _mailService.SendPasswordResetEmail(user, changePassword.ChangeRequestId.ToString());

            return BasicOperationResult<ChangePassword>.Ok(changePassword);
        }

        public async Task<IOperationResult<ChangePassword>> GenerateForgetPasswordRequest(string userEmail)
        {
            var user = await _userRepository.Find(u => u.Email == userEmail);

            return await GenerateChangePasswordRequest(user.Id);
        }
    }
}
