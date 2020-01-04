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

        public async Task<IOperationResult<Meeting>> CancelMeeting(int meetingId, int userId)
        {
            var user = await _userRepository.Find(u => u.Id == userId, u => u.UserRoles);

            if (user == null)
            {
                return BasicOperationResult<Meeting>.Fail("El usuario no fue encontrado");
            }

            var meeting = await _meetingRepository.Find(s => s.Id == meetingId);

            if (meeting == null)
            {
                return BasicOperationResult<Meeting>.Fail("La tutoría no fue encontrada");
            }

            if (!(meeting.StudentId == userId || meeting.TutorId == userId))
                return BasicOperationResult<Meeting>.Fail("El usuario no esta asociado a esta tutoría");


            meeting.Status = MeetingStatus.Cancelled;
            meeting.CancelerUserId = userId;
            var amount = CalculateMeetingAmount(meeting);
            _meetingRepository.Update(meeting);

            await _meetingRepository.Save();

            await _notificationManager.NotifyMeetingWasCanceled(meeting, userId, amount);

            return BasicOperationResult<Meeting>.Ok(meeting);
        }

        public async Task<IOperationResult<IEnumerable<Meeting>>> GetTutorMeetings(int tutorId)
        {
            var tutor = await _userRepository.Find(u => u.Id == tutorId, u => u.UserRoles);

            if (tutor == null)
            {
                return BasicOperationResult<IEnumerable<Meeting>>.Fail("El usuario no fue encontrado");
            }

            var meetings = await _meetingRepository.FindAll(u => u.TutorId == tutorId, u => u.Tutor, u => u.Subject);

            return BasicOperationResult<IEnumerable<Meeting>>.Ok(meetings);
        }

        public async Task<IOperationResult<IEnumerable<Meeting>>> GetStudentTutorMeetings(int userId)
        {
            IEnumerable<Meeting> meetings;
            var user = await _userRepository.Find(u => u.Id == userId, u => u.UserRoles);

            if (user == null)
            {
                return BasicOperationResult<IEnumerable<Meeting>>.Fail("El usuario no fue encontrado");
            }

            if (user.UserRoles.Any(ur => ur.RoleId == (int)RoleTypes.Tutor))
            {
                meetings = await _meetingRepository.FindAll(s => s.TutorId == userId);

            }
            else if (user.UserRoles.Any(ur => ur.RoleId == (int)RoleTypes.Student))
            {
                meetings = await _meetingRepository.FindAll(s =>  s.StudentId == userId);
            }
            else
            {
               return BasicOperationResult<IEnumerable< Meeting >>.Fail("El usuario no es un tutor o un estudiante");
            }

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

        public async Task<IOperationResult<Meeting>> StartMeeting(int meetingId, int userId)
        {
            var user = await _userRepository.Find(u => u.Id == userId, u => u.UserRoles);

            if (user == null)
            {
                return BasicOperationResult<Meeting>.Fail("El usuario no fue encontrado");
            }

            var meeting = await _meetingRepository.Find(s => s.Id == meetingId);

            if (meeting == null)
            {
                return BasicOperationResult<Meeting>.Fail("La tutoría no fue encontrada");
            }

            if (!(meeting.StudentId == userId || meeting.TutorId == userId))
                return BasicOperationResult<Meeting>.Fail("El usuario no esta asociado a esta tutoría");

            if (meeting.Status != MeetingStatus.Accepted)
                return BasicOperationResult<Meeting>.Fail("La tutoría aún no ha sido aceptada");

            meeting.Status = MeetingStatus.InProgress;
            meeting.RealStartedDateTime = DateTime.Now;
            _meetingRepository.Update(meeting);

            await _meetingRepository.Save();

            await _notificationManager.NotifyMeetingHasStarted(meeting);

            return BasicOperationResult<Meeting>.Ok(meeting);
        }

        public async Task<IOperationResult<Meeting>> EndMeeting(int meetingId, int userId)
        {
            var user = await _userRepository.Find(u => u.Id == userId, u => u.UserRoles);

            if (user == null)
            {
                return BasicOperationResult<Meeting>.Fail("El usuario no fue encontrado");
            }

            var meeting = await _meetingRepository.Find(s => s.Id == meetingId);

            if (meeting == null)
            {
                return BasicOperationResult<Meeting>.Fail("La tutoría no fue encontrada");
            }

            if (!(meeting.StudentId == userId || meeting.TutorId == userId))
                return BasicOperationResult<Meeting>.Fail("El usuario no esta asociado a esta tutoría");

            if (meeting.Status != MeetingStatus.InProgress)
                return BasicOperationResult<Meeting>.Fail("La tutoría aún no ha iniciado");

            meeting.Status = MeetingStatus.Complete;
            var amount = CalculateMeetingAmount(meeting);

            _meetingRepository.Update(meeting);

            await _meetingRepository.Save();
            
            await _notificationManager.NotifyMeetingCompleted(meeting, amount);

            return BasicOperationResult<Meeting>.Ok(meeting);
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


        public async Task<IOperationResult<Meeting>> GetCurrentMeeting(int userId)
        {
            var user = await _userRepository.Find(u => u.Id == userId, u => u.UserRoles);

            if (user == null)
            {
                return BasicOperationResult<Meeting>.Fail("El usuario no fue encontrado");
            }

            var meetingInCourse = await _meetingRepository
                .Find(m => (m.StudentId == userId || m.TutorId == userId) && m.Status == MeetingStatus.InProgress);

            if (meetingInCourse == null)
            {
                return BasicOperationResult<Meeting>.Fail("El usuario no tiene tutoría en curso");
            }

            return BasicOperationResult<Meeting>.Ok(meetingInCourse);
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
            if (!await TutorExistsAndIsTutor(meeting.TutorId))
            {
                return BasicOperationResult<Meeting>.Fail("El tutor no existe");
            }
            if (!await StudentExistsAndIsStudent(meeting.StudentId))
            {
                return BasicOperationResult<Meeting>.Fail("El estudiante no existe");
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
                .FirstOrDefaultAsync(u => u.UserRoles.Any(ur => ur.RoleId == (int)RoleTypes.Student)
                                          && u.Id == studentId && u.Id == studentId && u.IsActive && u.IsEmailValidated);
            if (student == null) return false;
            return true;
        }
        private async Task<bool> TutorExistsAndIsTutor(int tutorId)
        {
            var tutor = await _userRepository.Set
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserRoles.Any(ur => ur.RoleId == (int)RoleTypes.Tutor)
                                          && u.Id == tutorId && u.Id == tutorId && u.IsActive && u.IsEmailValidated);
            if (tutor == null) return false;

            return true;
        }

        public decimal CalculateMeetingAmount(Meeting meeting)
        {
            decimal result;
            decimal amountPerHour = 200.0m;

            decimal hours =meeting.EndDateTime.Hour - meeting.RealStartedDateTime.Hour;
            result = hours < 1 ? amountPerHour : amountPerHour * hours;

            return result;
        }


    }
}
