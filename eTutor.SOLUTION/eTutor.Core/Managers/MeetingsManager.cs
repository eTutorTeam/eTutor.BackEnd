using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Helpers;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using eTutor.Core.Validations;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Core.Managers
{
    public sealed class MeetingsManager: EntityBase
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMeetingRepository _meetingRepository;

        public MeetingsManager(ISubjectRepository subjectRepository, ITutorRepository tutorRepository,
            IUserRepository userRepository, IMeetingRepository meetingRepository)
        {
            _subjectRepository = subjectRepository;
            _tutorRepository = tutorRepository;
            _userRepository = userRepository;
            _meetingRepository = meetingRepository;
        }

        public async Task<IOperationResult<Meeting>> GetMeeting(int meetingId)
        {
            var meeting = await _meetingRepository.Find(s => s.Id == meetingId);

            if (meeting == null)
            {
                return BasicOperationResult<Meeting>.Fail("La tutoría no fue encontrada");
            }

            return BasicOperationResult<Meeting>.Ok(meeting);
        }

        public async Task<IOperationResult<IEnumerable<Meeting>>> GetTutorMeetings(int userId)
        {
            var meetings = await _meetingRepository.Set.Include(s => s.TutorId == userId).ToListAsync();

            return BasicOperationResult < IEnumerable < Meeting >>.Ok(meetings);
        }

        public async Task<IOperationResult<IEnumerable<Meeting>>> GetStudentMeetings(int userId)
        {
            var meetings = await _meetingRepository.Set.Include(s => s.StudentId == userId).ToListAsync();

            return BasicOperationResult<IEnumerable<Meeting>>.Ok(meetings);
        }

        public async Task<IOperationResult<Meeting>> CreateMeeting(Meeting meeting)
        {
            var validation = await ValidateMeeting(meeting);

            if (!validation.Success) return validation;

            var res = _meetingRepository.Create(meeting);

            if (!res.Success) return res;

            await _meetingRepository.Save();

            return BasicOperationResult<Meeting>.Ok(res.Entity);
        }


        private async Task<IOperationResult<Meeting>> ValidateMeeting(Meeting meeting)
        {
            var validator = new MeetingValidator();

            ValidationResult validationResult = await validator.ValidateAsync(meeting);

            if (!validationResult.IsValid)
            {
                return BasicOperationResult<Meeting>.Fail(validationResult.JSONFormatErrors());
            }

            return BasicOperationResult<Meeting>.Ok();
        }
    }
}
