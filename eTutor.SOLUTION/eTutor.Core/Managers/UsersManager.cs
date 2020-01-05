using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Helpers;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Int32;

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
        private readonly IFileService _fileService;
        private readonly IEmailValidationRepository _emailValidationRepository;
        private readonly int _maxFileSize;

        public UsersManager(SignInManager<User> signInManager, UserManager<User> userManager, IUserRepository userRepository, 
            IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IMailService mailService, 
            IParentStudentRepository parentStudentRepository, IFileService fileService, IConfiguration configuration, IEmailValidationRepository emailValidationRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mailService = mailService;
            _parentStudentRepository = parentStudentRepository;
            _fileService = fileService;
            _emailValidationRepository = emailValidationRepository;
            _maxFileSize = Parse(configuration.GetSection("Settings")["FileMaxSize"]);
        }

        public async Task<IOperationResult<User>> AuthenticateUser(string email, string password)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsNotAllowed)
                {
                    return BasicOperationResult<User>.Fail("El usuario no tiene permisos");
                }

                if (signInResult.IsLockedOut)
                {
                    return BasicOperationResult<User>.Fail("El usuario esta bloquedo");
                }

                if (signInResult.RequiresTwoFactor)
                {
                    return  BasicOperationResult<User>.Fail("El usuario requiere autenticacion de dos factores");
                }
                
                return BasicOperationResult<User>.Fail("El usuario no existe, revise su correo y contrasenia");
            }

            User user = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (!user.IsEmailValidated)
            {
                return BasicOperationResult<User>.Fail("Debe de validar su correo electrónico para continuar con el proceso de validar su cuenta");
            }

            if (!user.IsActive)
            {
                return BasicOperationResult<User>.Fail("El usuario debe de ser activado para poder acceder a su cuenta");
            }

            return BasicOperationResult<User>.Ok(user);
        }

        public async Task<IOperationResult<User>> RegisterTutorUser(User newUser, string password, ISet<RoleTypes> roles)
        {
            if (roles.Count() <= 0)
            {
                return BasicOperationResult<User>.Fail("No roles given to create user");
            }

            newUser.IsActive = false;
            IdentityResult userCreateResult = await _userManager.CreateAsync(newUser, password);

            if (!userCreateResult.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(userCreateResult.Errors));
            }
            
            User createdUser = await _userManager.FindByEmailAsync(newUser.Email);
            var emailValidation = await GetEmailToken(createdUser.Id);
            await _mailService.SendEmailToRegisteredUser(newUser, emailValidation.ValidationToken.ToString());

            IEnumerable<UserRole> userRoles = roles.Select(r => new UserRole {RoleId = (int) r, UserId = createdUser.Id});
            _userRoleRepository.Set.AddRange(userRoles);
            
            await _userRoleRepository.Save();
            
            User user = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == createdUser.Email);

            return BasicOperationResult<User>.Ok(user);
        }


        private async Task AsociateExistingParentWithNewStudent(User parentUser, User studentUser)
        {
            var roles = parentUser.UserRoles.Select(ur => ur.RoleId);

            if (roles.All(r => r != (int) RoleTypes.Parent))
            {
                var userRole = new UserRole
                {
                    RoleId = (int)RoleTypes.Parent,
                    UserId = parentUser.Id
                };
                _userRoleRepository.Create(userRole);
            }

            var parentStudentsResult = await _parentStudentRepository.FindAll(ps => ps.ParentId == parentUser.Id);
            var students = parentStudentsResult.Select(ps => ps.StudentId);

            if (students.Any(s => s == studentUser.Id))
            {
                return;
            }

            var parentStudent = new ParentStudent
            {
                ParentId = parentUser.Id,
                StudentId = studentUser.Id,
                Relationship = ParentRelationship.Father
            };

            _parentStudentRepository.Create(parentStudent);

            await _parentStudentRepository.Save();

            await _mailService.SendEmailToExistingParentToValidateStudent(studentUser, parentUser);

        }

        public async Task<IOperationResult<User>> RegisterStudentUser(User newUser, string password, string parentEmail)
        {
            if (string.IsNullOrEmpty(parentEmail))
            {
                return BasicOperationResult<User>.Fail("Debe suministrar el correo del padre, para continuar");
            }

            IdentityResult userCreateResult = await _userManager.CreateAsync(newUser, password);
            newUser.IsActive = false;

            if (!userCreateResult.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(userCreateResult.Errors));
            }

            User createdUser = await _userManager.FindByEmailAsync(newUser.Email);

            _userRoleRepository.Create(new UserRole {UserId = createdUser.Id, RoleId = (int) RoleTypes.Student});

            await _userRoleRepository.Save();

            User user = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == createdUser.Email);

            var emailToken = await GetEmailToken(createdUser.Id);
            await _mailService.SendEmailToCreatedStudentUser(createdUser, emailToken.ValidationToken.ToString());

            var parentUser = await _userRepository.Find(u => u.Email == parentEmail, u => u.UserRoles);

            if (parentUser != null)
            {
                await AsociateExistingParentWithNewStudent(parentUser, user);
            }
            else
            {
                await _mailService.SendEmailToParentToCreateAccountAndValidateStudent(createdUser, parentEmail);
            }

            return BasicOperationResult<User>.Ok(user);
        }

        public async Task<IOperationResult<User>> RegisterParentUser(User newUser, string password, int studentId)
        {
            var studentUser = await _userRepository.Set.Include(ur => ur.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == studentId);

            if (studentUser.UserRoles.All(ur => ur.RoleId != (int) RoleTypes.Student))
            {
                return BasicOperationResult<User>.Fail("El usuario al que intenta asociar este padre, no es un estudiantes");
            }

            newUser.IsActive = true;
            newUser.IsEmailValidated = true;
            var userCreateResult = await _userManager.CreateAsync(newUser, password);
            if (!userCreateResult.Succeeded)
            {
                return BasicOperationResult<User>.Fail(GetErrorsFromIdentityResult(userCreateResult.Errors));
            }

            var parentUser = await _userRepository.Set.FirstOrDefaultAsync(u => u.Email == newUser.Email);
            int userId = parentUser.Id;
            _userRoleRepository.Create(new UserRole {UserId = userId, RoleId = (int) RoleTypes.Parent});
            
            
            var parentStudent = new ParentStudent
            {
                ParentId = userId,
                StudentId = studentId,
                Relationship = ParentRelationship.Father
            };

            
            studentUser.IsActive = true;
            _userRepository.Update(studentUser);
            _parentStudentRepository.Create(parentStudent);
            await _parentStudentRepository.Save();

            await _mailService.SendEmailForSuccesfullAcountCreation(parentUser);
            await _mailService.SendEmailStudentActivated(studentUser);

            return BasicOperationResult<User>.Ok(parentUser);
        }

        public async Task<IEnumerable<Role>> GetRolesForUser(int userId) 
            => await _roleRepository.Set
                .Include(ur => ur.UserRoles)
                .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
                .ToListAsync();

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
                oldUser.AboutMe = user.AboutMe;
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

        public async Task<IOperationResult<User>> GetUserById(int userId)
        {
            var user = await _userRepository.Find(u => u.Id == userId);

            if (user == null)
            {
                return BasicOperationResult<User>.Fail("Los datos del usuario no fueron encontrados");
            }

            return BasicOperationResult<User>.Ok(user);
        }
        
        public async Task<IOperationResult<string>> UploadProfileImageForUser(int userId, string base64, string fileName)
        {
            var user = await _userRepository.Find(u => u.Id == userId);
            if (user == null)
            {
                return BasicOperationResult<string>.Fail("El usuario dado no fue encontrado");
            }

            byte[] fileBytes = Convert.FromBase64String(base64);

            var fileStream = new MemoryStream(fileBytes);
                
            if (!FileValidations.CheckIfFileIsImage(fileName))
            {
                return BasicOperationResult<string>.Fail("Debe de subir un archivo valido de imagen");
            }

            int fileSizeInKb = (int) (fileStream.Length / 1000);
            if (fileSizeInKb > _maxFileSize)
            {
                return BasicOperationResult<string>.Fail(
                    $"El archivo excede el limite de {_maxFileSize}kb en tamaño, intente con un archivo más pequeño");
            }

            fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(fileName)}";
            var fileUrl = await _fileService.UploadStreamToBucketServer(fileStream, fileName);
            if (string.IsNullOrEmpty(fileUrl))
            {
                return BasicOperationResult<string>.Fail("El archivo no pudo ser cargado al servidor");
            }

            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                //await _fileService.DeleteFileFromBucketServer(user.FileReference);
            }

            user.ProfileImageUrl = fileUrl;
            user.FileReference = fileName;
            _userRepository.Update(user);
            await _userRepository.Save();

            return BasicOperationResult<string>.Ok(fileUrl);
        }

        private async Task<EmailValidation> GetEmailToken(int userId)
        {

            EmailValidation emailValidation = await _emailValidationRepository.Find(e => e.UserId == userId);

            if (emailValidation == null)
            {
                emailValidation = new EmailValidation {ValidationToken = Guid.NewGuid(), UserId = userId};
                _emailValidationRepository.Create(emailValidation);
                await _emailValidationRepository.Save();
            }

            return emailValidation;
        }

       
        public async Task<IOperationResult<IEnumerable<User>>> GetStudentsByParentId(int userId)
        {
            var parentStudents = await _parentStudentRepository
                .FindAll(x => x.ParentId == userId, 
                    x => x.Student);

            if (parentStudents == null || !parentStudents.Any())
            {
                return BasicOperationResult<IEnumerable<User>>.Fail("Los datos del usuario no fueron encontrados");
            }
            
            var studentsUsers = parentStudents.Select(ps => ps.Student);
            
            return BasicOperationResult<IEnumerable<User>>.Ok(studentsUsers);
        }
        public async Task<IOperationResult<bool>> ToggleUserAccountState(int userId)
        {
            try
            {
                var oldUser = await _userRepository.Find(u => u.Id == userId);

                oldUser.IsActive = !oldUser.IsActive;

                _userRepository.Update(oldUser);

                await _userRepository.Save();

                return BasicOperationResult<bool>.Ok(true);
            }
            catch (Exception e)
            {
                return BasicOperationResult<bool>.Fail(e.Message);
            }
        }
    }
}