using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Core.Managers
{
    public class ParentAuthorizationManager
    {
        private readonly IParentAuthorizationRepository _parentAuthorizationRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly INotificationService _notificationService;
        private readonly NotificationManager _notificationManager;
        private Dictionary<ParentAuthorizationStatus, Func<Meeting, Task>> _notificationActions;

        public ParentAuthorizationManager(IParentAuthorizationRepository parentAuthorizationRepository,
            IMeetingRepository meetingRepository, INotificationService notificationService, 
            NotificationManager notificationManager)
        {
            _parentAuthorizationRepository = parentAuthorizationRepository;
            _meetingRepository = meetingRepository;
            _notificationService = notificationService;
            _notificationManager = notificationManager;
            _notificationActions = new Dictionary<ParentAuthorizationStatus, Func<Meeting, Task>>
            {
                {ParentAuthorizationStatus.Approved, NotifyStuentAndTutorOfApprovedMeeting},
                {ParentAuthorizationStatus.Rejected, NotifyStudentOfRejectedMeeting}
            };
        }

        public async Task<IOperationResult<ParentAuthorization>> CreateParentAuthorization(int meetingId, int parentId, ParentAuthorization answer)
        {
            var meeting = await _meetingRepository.Find(m => m.Id == meetingId,
                m => m.Tutor, m => m.Student, m => m.Subject);

            if (meeting == null)
            {
                return BasicOperationResult<ParentAuthorization>.Fail("La tutoría agendada no fue encontrada");
            }
            
            var authorization = new ParentAuthorization
            {
                AuthorizationDate = DateTime.Now,
                ParentId = answer.ParentId,
                Status = answer.Status
            };

            _parentAuthorizationRepository.Create(authorization);
            await _parentAuthorizationRepository.Save();

            meeting.ParentAuthorizationId = authorization.Id;
            meeting.Status = authorization.Status == ParentAuthorizationStatus.Approved
                ? MeetingStatus.Approved
                : MeetingStatus.Rejected;
            _meetingRepository.Update(meeting);

            await _meetingRepository.Save();

            await _notificationActions[authorization.Status](meeting);
            
            return BasicOperationResult<ParentAuthorization>.Ok(authorization);
        }

        private async Task NotifyStuentAndTutorOfApprovedMeeting(Meeting meeting)
        {
            var student = meeting.Student;
            var tutor = meeting.Tutor;

            await _notificationManager.NotifyTutorOfSolicitedMeeting(tutor.Id, meeting.Subject, student, meeting.Id);

            string studentMessage =
                $"Su padre ha autorizado su tutoría con {tutor.FullName} para la materia {meeting.Subject.Name}";

            string title = "Tutoría Aprobada";

            await _notificationService.SendNotificationToUser(student, studentMessage, title);
        }

        private async Task NotifyStudentOfRejectedMeeting(Meeting meeting)
        {
            var student = meeting.Student;

            string message = $"Su tutoría con {meeting.Tutor.FullName} ha sido rechazada por su padre.";

            string title = "Tutoría Denegada";

            await _notificationService.SendNotificationToUser(student, message, title);
        }
        
    }
}