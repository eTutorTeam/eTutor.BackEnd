using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Core.Managers
{
    public class UsersManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMailService _mailService;

        public UsersManager(SignInManager<User> signInManager, UserManager<User> userManager, IUserRepository userRepository, 
            IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IMailService mailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mailService = mailService;
        }


        public async Task<IOperationResult<User>> AuthenticateUser(string email, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsNotAllowed)
                {
                    return BasicOperationResult<User>.Fail("User is not allowed");
                }

                if (signInResult.IsLockedOut)
                {
                    return BasicOperationResult<User>.Fail("User is locked out");
                }

                if (signInResult.RequiresTwoFactor)
                {
                    return  BasicOperationResult<User>.Fail("User requires two factor authentication");
                }
                
                return BasicOperationResult<User>.Fail("User may not exist, check your email and password");
            }

            User user = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == email);

            return BasicOperationResult<User>.Ok(user);
        }


        public async Task<IOperationResult<User>> RegisterUser(User newUser, string password, ISet<RoleTypes> roles)
        {
            if (roles.Count() <= 0)
            {
                return BasicOperationResult<User>.Fail("No roles given to create user");
            }
            
            IdentityResult userCreateResult = await _userManager.CreateAsync(newUser, password);

            if (!userCreateResult.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(userCreateResult.Errors));
            }
            
            User createdUser = await _userManager.FindByEmailAsync(newUser.Email);

            IEnumerable<UserRole> userRoles = roles.Select(r => new UserRole {RoleId = (int) r, UserId = createdUser.Id});
            _userRoleRepository.Set.AddRange(userRoles);
            
            await _userRoleRepository.Save();
            
            User user = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == createdUser.Email);

            return BasicOperationResult<User>.Ok(user);
            
        }

        public async Task<IEnumerable<Role>> GetRolesForUser(int userId) 
            => await _roleRepository.Set
                .Include(ur => ur.UserRoles)
                .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
                .ToListAsync();


        private string GetErrorsFromIdentityResult(IEnumerable<IdentityError> errors)
        {
            string text = string.Empty;

            for (int i = 0; i < errors.Count(); i++)
            {
                if (i > 0) text += ", ";
                    var error = errors.ElementAt(i);
                text += $"({error.Code}) {error.Description}";
                
            }

            return text;
        }

        public async Task<IOperationResult<string>> UserForgotPassword(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BasicOperationResult<string>.Fail($"There's no user registered with the email {email}");
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _mailService.SendPasswordResetEmail(user, passwordResetToken);

            if (!result.Success)
            {
                return BasicOperationResult<string>.Fail("There was a problem, and couldn't send the email");
            }

            return BasicOperationResult<string>.Ok("Email Sent");
        }

        public async Task<IOperationResult<User>> ChangePasswordUserForgot(int userId, string newPassword, string token)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(result.Errors));
            }

            return BasicOperationResult<User>.Ok(user);

        }

        public async Task<IOperationResult<User>> ChangePassword(int userId, string newPassword, string currentPassword)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());

            var result =
                await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(result.Errors));
            }

            return BasicOperationResult<User>.Ok(user);
        }

        public async Task<IOperationResult<IEnumerable<Role>>> GetAllRoles()
        {
            var roles = await _roleRepository.Set
                .OrderBy(r => r.Name)
                .ToListAsync();

            if (roles.Count == 0)
            {
                return BasicOperationResult<IEnumerable<Role>>.Fail("No roles where found");
            }

            return BasicOperationResult<IEnumerable<Role>>.Ok(roles);
        }
    }
}