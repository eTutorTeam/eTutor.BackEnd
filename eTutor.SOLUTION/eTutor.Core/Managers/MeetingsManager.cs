using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private readonly ISubjectRepository _subjectRepository;
        private readonly NotificationManager _notificationManager;
        private readonly IUserRepository _userRepository;

        public MeetingsManager(IMeetingRepository meetingRepository,
            ISubjectRepository subjectRepository, IUserRepository userRepository,
            NotificationManager notificationManager)
        {
            _meetingRepository = meetingRepository;
            _subjectRepository = subjectRepository;
            _userRepository = userRepository;
            _notificationManager = notificationManager;
        }

        public async Task<IOperationResult<Meeting>> GetMeeting(int meetingId, int userId)
        {
            var meeting = await _meetingRepository.Find(s => s.Id == meetingId && (s.StudentId == userId || s.TutorId == userId));

            if (meeting == null)
            {
                return BasicOperationResult<Meeting>.Fail("La tutoría no fue encontrada");
            }

            return BasicOperationResult<Meeting>.Ok(meeting);
        }
        
        public async Task<IOperationResult<Meeting>> GetMeetingForParent(int meetingId, int parentId)
        {
            var meeting = await _meetingRepository.GetMeetingForParent(meetingId, parentId);

            if (meeting == null)
            {
                return BasicOperationResult<Meeting>.Fail("La tutoría agendada no fue encontrada");
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

            if (meeting == null)
            {
                return BasicOperationResult<Meeting>.Fail("El objeto que envió es inválido");
            }
            
            var validation = await ValidateMeeting(meeting);

            if (!validation.Success) return validation;

            meeting.Status = MeetingStatus.Pending;
            
            _meetingRepository.Create(meeting);

            await _meetingRepository.Save();

            var response = await _meetingRepository.Find(m => m.Id == meeting.Id, m => m.Subject, m => m.Tutor, m => m.Student);

            await _notificationManager.NotifyStudentMeetingWasCreated(meeting.StudentId, meeting.Subject.Name, meeting.Tutor.FullName);

            await _notificationManager.NotifyParentsOfMeetingCreatedForStudent(meeting);

            return BasicOperationResult<Meeting>.Ok(response);
        }

        public async Task<IOperationResult<Meeting>> GetTutorMeetingSummary(int meetingId, int tutorId)
        {
            var meeting = await _meetingRepository.Find(
                m => m.Id == meetingId && m.TutorId == tutorId,
                m => m.Student, m => m.Subject
                     );

            if (meeting == null)
            {
                return BasicOperationResult<Meeting>.Fail("La solicitud no fue encontrada");
            }

            var validateResult = await ValidateMeeting(meeting);
            if (!validateResult.Success)
            {
                return validateResult;
            }
            
            return BasicOperationResult<Meeting>.Ok(meeting);

        }
        
        public async Task<IOperationResult<string>> TutorResponseToMeetingRequest(int meetingId, MeetingStatus answeredStatusAnsweredStatus, int userId)
        {
            var meeting = await FindMeetingWithTutor(meetingId, userId);

            if (meeting == null)
            {
                return BasicOperationResult<string>.Fail("La tutoría agendada a la que intenta responder no existe");
            }

            MeetingStatus status = answeredStatusAnsweredStatus;
            if (status != MeetingStatus.Accepted && status != MeetingStatus.Rejected)
            {
                return BasicOperationResult<string>.Fail("No tiene los permisos para dar ese tipo de respuesta");
            }

            meeting.Status = status;

            _meetingRepository.Update(meeting);

            await _meetingRepository.Save();

            string responseMessage = status == MeetingStatus.Accepted
                ? "La tutoría ha sido aceptada exitosamente"
                : "La tutoría ha sido rechazada exitosamente";

            await _notificationManager.NotifySolicitedMeetingByStudentAnswered(meeting);
            
            return BasicOperationResult<string>.Ok(responseMessage);
        }


        public async Task<IOperationResult<ISet<Meeting>>> GetFutureParentMeetings(int parentId)
        {
            var parentExists = await _userRepository.Exists(u =>
                    u.Id == parentId && u.UserRoles.Any(ur => ur.RoleId == (int) RoleTypes.Parent),
                u => u.UserRoles
            );

            if (!parentExists)
            {
                return BasicOperationResult<ISet<Meeting>>.Fail("Este padre no existe en nuestra base de datos");
            }
            
            var meetings = await _meetingRepository.GetAllMeetingsOfParentStudents(parentId);
            var filteredMeetings = meetings.Where(
                m => m.StartDateTime > DateTime.Now.AddHours(-2)
                     && m.ParentAuthorizationId == null
                     && m.Status == MeetingStatus.Pending
            ).ToHashSet();
            
            return BasicOperationResult<ISet<Meeting>>.Ok(filteredMeetings);
        }


        private async Task<Meeting> FindMeetingWithTutor(int meetingId, int tutorId)
        {
            var meeting = await _meetingRepository.Find(
                m => m.Id == meetingId && m.TutorId == tutorId,
                m => m.Student, m => m.Subject
            );

            return meeting;
        }

        private async Task<IOperationResult<Meeting>> ValidateMeeting(Meeting meeting)
        {
            var validator = new MeetingValidator();

            ValidationResult validationResult = await validator.ValidateAsync(meeting);

            if (!validationResult.IsValid)
            {
                return BasicOperationResult<Meeting>.Fail(validationResult.JSONFormatErrors());
            }
            
            if (!await SubjectExists(meeting.SubjectId))
            {
                return BasicOperationResult<Meeting>.Fail("La materia inidicada en la solicitud no existe");
            }

            if (!await SubjectExists(meeting.SubjectId))
            {
                return BasicOperationResult<Meeting>.Fail("La materia no existe");
            }

            if (!await StudentExistsAndIsStudent(meeting.StudentId))
            {
                return BasicOperationResult<Meeting>.Fail("El estudiante indicado no existe");
            }
            
            if (!await TutorExistsAndIsTutor(meeting.TutorId))
            {
                return BasicOperationResult<Meeting>.Fail("El tutor indicado no existe");
            }
            
            return BasicOperationResult<Meeting>.Ok(meeting);
        }

        private async Task<bool> SubjectExists(int subjectId)
        {
            return await _subjectRepository.Exists(s => s.Id == subjectId);
        }

        private async Task<bool> StudentExistsAndIsStudent(int studentId)
        {
            var student = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserRoles.Any(ur => ur.RoleId == (int)RoleTypes.Student) && u.Id == studentId);
            if (student == null) return false;
            return true;
        }
        
        private async Task<bool> TutorExistsAndIsTutor(int tutorId)
        {
            return await _userRepository.Exists(s => s.Id == tutorId 
                                                     && s.IsActive && s.IsEmailValidated
                                                     && s.UserRoles.Any(ur => ur.RoleId == (int)RoleTypes.Tutor), s => s.UserRoles);
        }
    }
}
