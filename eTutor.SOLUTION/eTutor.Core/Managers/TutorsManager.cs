using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Core.Managers
{
    public sealed class TutorsManager
    {
        private readonly IUserRepository _userRepository;

        public TutorsManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<IOperationResult<IEnumerable<User>>> GetListOfTutorsFilteredByIsActive(bool isActive = true)
        {
            try
            {
                IEnumerable<User> tutors = await _userRepository.Set
                    .Include(u => u.UserRoles)
                    .Where(u => u.UserRoles.Any(ur => ur.RoleId == (int) RoleTypes.Tutor) && u.IsActive == isActive)
                    .ToListAsync();

                return BasicOperationResult<IEnumerable<User>>.Ok(tutors);
            }
            catch (Exception e)
            {
                return BasicOperationResult<IEnumerable<User>>.Fail(e.Message);
            }
        }

        public async Task<IOperationResult<User>> ActivateUserForTutor(int tutorId)
        {
            var tutor = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserRoles.Any(ur => ur.RoleId == (int) RoleTypes.Tutor) && u.Id == tutorId);

            if (tutor == null)
            {
                return BasicOperationResult<User>.Fail("El usuario no fue encontrado");
            }

            return BasicOperationResult<User>.Ok(tutor);

        }
    }
}
