using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace eTutor.Core.Managers
{
    public class AccountsManager
    {
        private readonly IChangePasswordRepository _changePasswordRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        public AccountsManager(IChangePasswordRepository changePasswordRepository,
            IUserRepository userRepository, IMailService mailService,
            UserManager<User> userManager)
        {
            _changePasswordRepository = changePasswordRepository;
            _userRepository = userRepository;
            _mailService = mailService;
            _userManager = userManager;
        }

        public async Task<IOperationResult<ChangePassword>> GenerateChangePasswordRequest(int userId)
        {
            var user = await _userRepository.Find(u => u.Id == userId);

            if (user == null)
            {
                return BasicOperationResult<ChangePassword>.Fail("El usuario no fue encontrado");
            }

            if (!user.IsActive)
            {
                return BasicOperationResult<ChangePassword>.Fail("Este usuario está inactivo, debe de solicitar la activación de su cuenta para continuar.");
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            ChangePassword changePassword = new ChangePassword
            {
                ChangeRequestId = Guid.NewGuid(), 
                UserId = userId,
                ChangeToken = token
            };

            _changePasswordRepository.Create(changePassword);

            await _changePasswordRepository.Save();
            await _mailService.SendPasswordResetEmail(user, changePassword.ChangeRequestId.ToString());

            return BasicOperationResult<ChangePassword>.Ok(changePassword);
        }
        
        public async Task<IOperationResult<ChangePassword>> CheckIfChangePasswordRequestIsValid(Guid changePasswordId)
        {
            var changeRequest = await _changePasswordRepository.Find(c => c.ChangeRequestId == changePasswordId);

            if (changeRequest == null)
            {
                return BasicOperationResult<ChangePassword>.Fail("Este enlace no existe");
            }

            if (changeRequest.ExpirationDate <= DateTime.Now)
            {
                return BasicOperationResult<ChangePassword>.Fail("Ya pasó el plazo de 24 horas para este enlace");
            }

            if (changeRequest.IsUsed)
            {
                return BasicOperationResult<ChangePassword>.Fail("Este enlace ya ha sido utilizado");
            }
            
            return BasicOperationResult<ChangePassword>.Ok(changeRequest);
        }

        public async Task<IOperationResult<ChangePassword>> GenerateForgetPasswordRequest(string userEmail)
        {
            var user = await _userRepository.Find(u => u.Email == userEmail);
            
            if (user == null)
            {
                return BasicOperationResult<ChangePassword>.Fail("No fue encontrado un usuario, con el correo electrónico solicitado");
            }

            return await GenerateChangePasswordRequest(user.Id);
        }

        public async Task<IOperationResult<bool>> ChangePasswordWithChangeRequestId(Guid changePasswordId, string password, string confirmPassword)
        {
            var result = await CheckIfChangePasswordRequestIsValid(changePasswordId);

            if (!result.Success)
            {
                return BasicOperationResult<bool>.Fail(result.Message.Message);
            }

            ChangePassword changeRequest = result.Entity;

            if (password != confirmPassword)
            {
                return BasicOperationResult<bool>.Fail("Las contraseñas deben de ser iguales.");
            }

            var user = await _userRepository.Find(u => u.Id == changeRequest.UserId);

            var identityResult = await _userManager.ResetPasswordAsync(user, changeRequest.ChangeToken, password);

            if (!identityResult.Succeeded)
            {
                return BasicOperationResult<bool>.Fail("La contraseña no pudo ser actualizada, debido a un error");
            }
            
            changeRequest.IsUsed = true;

            _changePasswordRepository.Update(changeRequest);
            await _changePasswordRepository.Save();
            
            return BasicOperationResult<bool>.Ok(true);
        }

        public async Task<IOperationResult<bool>> ChangePasswordForUser(int userId, string currentPassword,
            string newPassword)
        {
            var user = await _userRepository.Find(u => u.Id == userId);

            if (user == null)
            {
                return BasicOperationResult<bool>.Fail("El usuario indicado no existe");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                return BasicOperationResult<bool>.Fail("Ingresó una contraseña incorrecta");
            }
            
            return BasicOperationResult<bool>.Ok();
        }

        public async Task<IOperationResult<string>> ValidateEmailForUser(int userId)
        {
            return BasicOperationResult<string>.Ok("El correo electornico ha sido validado exitosamente");
        }
    }
}
