using System;
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
        private readonly IParentStudentRepository _parentStudentRepository;
        private readonly IMailService _mailService;

        public UsersManager(SignInManager<User> signInManager, UserManager<User> userManager, IUserRepository userRepository, 
            IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IMailService mailService, IParentStudentRepository parentStudentRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mailService = mailService;
            _parentStudentRepository = parentStudentRepository;
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

            if (!user.IsActive)
            {
                return BasicOperationResult<User>.Fail("The user still needs to be activated");
            }

            return BasicOperationResult<User>.Ok(user);
        }

        public async Task<IOperationResult<User>> RegisterUser(User newUser, string password, ISet<RoleTypes> roles)
        {
            if (roles.Count() <= 0)
            {
                return BasicOperationResult<User>.Fail("No roles given to create user");
            }

            newUser.IsActive = true;
            IdentityResult userCreateResult = await _userManager.CreateAsync(newUser, password);

            if (!userCreateResult.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(userCreateResult.Errors));
            }
            
            User createdUser = await _userManager.FindByEmailAsync(newUser.Email);
            await _mailService.SendEmailToRegisteredUser(newUser);

            IEnumerable<UserRole> userRoles = roles.Select(r => new UserRole {RoleId = (int) r, UserId = createdUser.Id});
            _userRoleRepository.Set.AddRange(userRoles);
            
            await _userRoleRepository.Save();
            
            User user = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == createdUser.Email);

            return BasicOperationResult<User>.Ok(user);
        }

        public async Task<IOperationResult<User>> RegisterStudentUser(User newUser, string password, string parentEmail)
        {
            IdentityResult userCreateResult = await _userManager.CreateAsync(newUser, password);
            newUser.IsActive = false;

            if (!userCreateResult.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(userCreateResult.Errors));
            }

            User createdUser = await _userManager.FindByEmailAsync(newUser.Email);
            await _mailService.SendEmailToCreatedStudentUser(newUser);
            await _mailService.SendEmailToParentToCreateAccountAndValidateStudent(newUser, parentEmail);

            _userRoleRepository.Create(new UserRole {UserId = createdUser.Id, RoleId = (int) RoleTypes.Student});

            await _userRoleRepository.Save();

            User user = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == createdUser.Email);

            return BasicOperationResult<User>.Ok(user);
        }

        public async Task<IOperationResult<User>> RegisterParentUser(User newUser, string password, int studentId)
        {
            var userCreateResult = await _userManager.CreateAsync(newUser, password);
            newUser.IsActive = true;

            if (!userCreateResult.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(userCreateResult.Errors));
            }

            var user = await _userRepository.Set.FirstOrDefaultAsync(u => u.Email == newUser.Email);
            int userId = user.Id;
            _userRoleRepository.Create(new UserRole {UserId = userId, RoleId = (int) RoleTypes.Parent});
            await _mailService.SendEmailForSuccesfullAcountCreation(user);
            
            var parentStudent = new ParentStudent
            {
                ParentId = userId,
                StudentId = studentId
            };

            var studentUser = await _userRepository.Find(u => u.Id == studentId);
            studentUser.IsActive = true;
            _userRepository.Update(studentUser);
            _parentStudentRepository.Create(parentStudent);
            await _parentStudentRepository.Save();

            await _mailService.SendEmailStudentActivated();

            return BasicOperationResult<User>.Ok(user);
        }

        public async Task<IEnumerable<Role>> GetRolesForUser(int userId) 
            => await _roleRepository.Set
                .Include(ur => ur.UserRoles)
                .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
                .ToListAsync();

        public async Task<IOperationResult<string>> UserForgotPassword(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BasicOperationResult<string>.Fail($"There's no user registered with the email {email}");
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _mailService.SendPasswordResetEmail(user, passwordResetToken);

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

        public async Task<IOperationResult<User>> GetUserProfile(int userId)
        {
            var user = await _userRepository.Set.Include(u => u.UserRoles).FirstAsync(u => u.Id == userId);

            if (user == null)
            {
                return BasicOperationResult<User>.Fail("User was not found");
            }

            return BasicOperationResult<User>.Ok(user);
        }

        public async Task<IOperationResult<bool>> UpdateUserProfile(User user, int userId)
        {
            try
            {
                var oldUser = await _userRepository.Find(u => u.Id == userId);

                oldUser.Address = user.Address;
                oldUser.Name = user.Name;
                oldUser.LastName = user.LastName;
                oldUser.Gender = user.Gender;
                oldUser.Latitude = user.Latitude;
                oldUser.Longitude = user.Longitude;
                oldUser.Email = user.Email;
                oldUser.PersonalId = user.PersonalId;
                oldUser.UserName = user.UserName;
                oldUser.NormalizedUserName = user.UserName.ToUpper();
                oldUser.NormalizedEmail = user.Email.ToUpper();

                _userRepository.Update(oldUser);

                await _userRepository.Save();

                return BasicOperationResult<bool>.Ok(true);
            }
            catch (Exception e)
            {
                return BasicOperationResult<bool>.Fail(e.Message);
            }
        }

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

    }
}