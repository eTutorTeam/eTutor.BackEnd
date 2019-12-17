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
        private readonly IMeetingRepository _meetingRepository;

        public MeetingsManager(IMeetingRepository meetingRepository)
        {
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
            var meetings = await _meetingRepository.FindAll(u => u.TutorId == userId, u => u.Tutor, u => u.Subject);

            return BasicOperationResult<IEnumerable<Meeting>>.Ok(meetings);
        }

        public async Task<IOperationResult<IEnumerable<Meeting>>> GetStudentMeetings(int userId)
        {
            var meetings = await _meetingRepository.FindAll(u => u.StudentId == userId, u => u.Student, u => u.Tutor);

            return BasicOperationResult<IEnumerable<Meeting>>.Ok(meetings);
        }

        public async Task<IOperationResult<Meeting>> CreateMeeting(Meeting meeting)
        {
            var validation = await ValidateMeeting(meeting);

            if (!validation.Success) return validation;

            meeting.Status = MeetingStatus.Pending;
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
