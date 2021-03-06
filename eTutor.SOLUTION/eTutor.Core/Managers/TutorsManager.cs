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
        private readonly IMeetingRepository _meetingRepository;
        private readonly IRejectedMeetingRepository _rejectedMeetingRepository;

        public TutorsManager(IUserRepository userRepository, 
            ITutorSubjectRepository tutorSubjectRepository, ISubjectRepository subjectRepository, 
            IMeetingRepository meetingRepository, IRejectedMeetingRepository rejectedMeetingRepository)
        {
            _userRepository = userRepository;
            _tutorSubjectRepository = tutorSubjectRepository;
            _subjectRepository = subjectRepository;
            _meetingRepository = meetingRepository;
            _rejectedMeetingRepository = rejectedMeetingRepository;
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

        public async Task<IOperationResult<User>> GetTutorById(int tutorId)
        {
            var tutor = await _userRepository.Find(t =>
                    t.Id == tutorId && t.IsEmailValidated && t.IsActive
                    && t.UserRoles.Any(ur => ur.RoleId == (int) RoleTypes.Tutor),
                t => t.UserRoles
            );

            if (tutor == null)
            {
                return BasicOperationResult<User>.Fail("El tutor solicitado no fue encontrado, o ha sido inhabilitado en nuestro sistema");
            }
            
            return BasicOperationResult<User>.Ok(tutor);
        }

        public async Task<IOperationResult<ISet<User>>> GetTutorNotInCurMeeting(int meetingId)
        {
            var meeting = await _meetingRepository.Find(
                m => m.Id == meetingId && m.Status == MeetingStatus.Rejected,
                m => m.Subject, m => m.Subject.Tutors);

            if (meeting == null)
            {
                return BasicOperationResult<ISet<User>>.Fail("La tutoría rechazada no fue encontrada");
            }

            var tutorsResult = await GetTutorsBySubjectId(meeting.SubjectId);

            if (!tutorsResult.Success)
            {
                return tutorsResult;
            }

            var rejections = await _rejectedMeetingRepository.FindAll(r => r.MeetingId == meeting.Id);

            ISet<User> tutors = tutorsResult
                .Entity
                .Where(t => t.Id != meeting.TutorId && rejections.All(r => r.TutorId != t.Id))
                .ToHashSet();

            if (!tutors.Any())
            {
                return BasicOperationResult<ISet<User>>.Fail("No hay más tutores disponibles para esta matería, lo sentimos, estamos trabajando por obtener más");
            }
            
            return BasicOperationResult<ISet<User>>.Ok(tutors);

        }

        public async Task<IOperationResult<User>> GetRandomNotUsedTutorForMeeting(int meetingId)
        {
            var meeting = await _meetingRepository.Find(m => m.Id == meetingId);

            if (meeting == null)
            {
                return BasicOperationResult<User>.Fail("La tutoría no fue encontrada");
            }
            
            var tutorsResult = await GetTutorsBySubjectId(meeting.SubjectId);

            if (!tutorsResult.Success)
            {
                return BasicOperationResult<User>.Fail(tutorsResult.Message.Message);
            }

            var rejections = await _rejectedMeetingRepository.FindAll(r => r.MeetingId == meetingId);

            var tutors = tutorsResult.Entity.Where(t => rejections.All(r => r.TutorId != t.Id));
            
            if (!tutors.Any())
            {
                return BasicOperationResult<User>.Fail("No hay más tutores disponibles para esta matería, lo sentimos, estamos trabajando por obtener más");
            }
            
            var rand = new Random();
            int index = rand.Next(tutors.Count());

            var selectedTutor = tutors.ElementAtOrDefault(index);

             if (selectedTutor == null)
            {
                return BasicOperationResult<User>.Fail("No se pudo seleccionar ningún tutor de manera aleatoria");
            }
            
            return BasicOperationResult<User>.Ok(selectedTutor);
        }
    }
}
