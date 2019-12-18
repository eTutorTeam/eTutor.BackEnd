﻿using System;
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
        private readonly ITutorSubjectRepository _tutorSubjectRepository;
        private readonly ISubjectRepository _subjectRepository;

        public TutorsManager(IUserRepository userRepository, 
            ITutorSubjectRepository tutorSubjectRepository, ISubjectRepository subjectRepository)
        {
            _userRepository = userRepository;
            _tutorSubjectRepository = tutorSubjectRepository;
            _subjectRepository = subjectRepository;
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

        public async Task<IOperationResult<User>> ActivateUserForTutor(int tutorId, bool activate = true)
        {
            var tutor = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserRoles.Any(ur => ur.RoleId == (int) RoleTypes.Tutor) && u.Id == tutorId);

            if (tutor == null)
            {
                return BasicOperationResult<User>.Fail("El usuario no fue encontrado");
            }

            tutor.IsActive = activate;

            _userRepository.Update(tutor);

            await _userRepository.Save();

            return BasicOperationResult<User>.Ok(tutor);

        }

        public async Task<IOperationResult<ISet<User>>> GetTutorsBySubjectId(int subjectId)
        {
            var subjectExists = await _subjectRepository.Exists(s => s.Id == subjectId);

            if (!subjectExists)
            {
                return BasicOperationResult<ISet<User>>.Fail("La materia que por la que intenta buscar tutores no existe en el sistema ya");
            }
            
            var tutorList = await _tutorSubjectRepository.FindAll(t => t.SubjectId == subjectId, 
                t => t.Tutor);

            var tutors = tutorList.Select(t => t.Tutor).Where(t => t.IsActive && t.IsEmailValidated).ToHashSet();
            
            return BasicOperationResult<ISet<User>>.Ok(tutors);


        }
    }
}
