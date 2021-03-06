using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Core.Managers
{
    public sealed class ParentsManager
    {
    
        private readonly IUserRepository _userRepository;
        private readonly IParentStudentRepository _parentStudentRepository;
        private readonly IMailService _mailService;

        public ParentsManager(IUserRepository userRepository, 
            IParentStudentRepository parentStudentRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _parentStudentRepository = parentStudentRepository;
            _mailService = mailService;
        }
        
        public async Task<IOperationResult<IEnumerable<User>>> GetStudentsByParentId(int parentId)
        {
            var parentStudents = await _parentStudentRepository
                .FindAll(x => x.ParentId == parentId, 
                    x => x.Student);

            if (parentStudents == null || !parentStudents.Any())
            {
                return BasicOperationResult<IEnumerable<User>>.Fail("Los datos del usuario no fueron encontrados");
            }
            
            var studentsUsers = parentStudents.Select(ps => ps.Student);
            
            return BasicOperationResult<IEnumerable<User>>.Ok(studentsUsers);
        }
        
        public async Task<IOperationResult<bool>> ToggleStudentAccountActivation(int studentId, int parentId)
        {
            var oldUser = await _userRepository.Find(
                u => u.Id == studentId && u.UserRoles.Any(ur => ur.RoleId == (int) RoleTypes.Student),
                u => u.UserRoles, u => u.Parents);

            if (oldUser == null)
            {
                return BasicOperationResult<bool>.Fail("El estudiante no fue encontrado");
            }

            if (oldUser.Parents.All(p => p.ParentId != parentId))
            {
                return BasicOperationResult<bool>.Fail("El estudiante no esta relacionado con el Padre indicado");
            }

            oldUser.IsActive = !oldUser.IsActive;

            _userRepository.Update(oldUser);

            await _userRepository.Save();

            await _mailService.SendWhenAccountStateToggled(oldUser);

            return BasicOperationResult<bool>.Ok(true);
        }
    }
}