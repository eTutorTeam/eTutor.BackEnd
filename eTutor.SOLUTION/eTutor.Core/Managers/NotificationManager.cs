using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using Newtonsoft.Json;

namespace eTutor.Core.Managers
{
    public class NotificationManager
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public NotificationManager(IUserRepository userRepository, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
        }


        public async Task<IOperationResult<string>> NotifyStudentMeetingWasCreated(int userId, string subject, string tutor)
        {
            var userResult = await GetUser(userId);

            if (!userResult.Success) return BasicOperationResult<string>.Fail(userResult.Message.Message);

            var user = userResult.Entity;

            string message =
                $"Su tutoria de {subject} ha sido creada, le avisamos cuando {tutor} acepte la solicitud";

            await _notificationService.SendNotificationToUser(user, message, "Tutoria Creada");
            
            return BasicOperationResult<string>.Ok("Tutoria Creada");
        }
        
        public async Task<IOperationResult<string>> NotifyTutorOfSolicitedMeeting(int tutorId, Subject subject, User student, int meetingId)
        {
            var userResult = await GetUser(tutorId);

            if (!userResult.Success) return BasicOperationResult<string>.Fail(userResult.Message.Message);

            var user = userResult.Entity;

            string message =
                $"Una tutoria ha sido solicitada de {subject.Name}, por el estudiante {student.FullName}";

            var data = new Dictionary<string, string>
            {
                {"newSolicitedMeetingId", meetingId.ToString()}
            };

            await _notificationService.SendNotificationToUser(user, message, $"Tutoria Solicitada: {subject.Name}", data);
            
            return BasicOperationResult<string>.Ok("Tutoria Creada");
        }
        
        public async Task<IOperationResult<string>> NotifySolicitedMeetingByStudentAnswered(Meeting meeting)
        {
            var tutorResult = await GetUser(meeting.TutorId);

            if (!tutorResult.Success)
            {
                return BasicOperationResult<string>.Fail(tutorResult.Message.Message);
            }

            User tutorUser = tutorResult.Entity;

            string message = meeting.Status == MeetingStatus.Accepted
                ? $"Su tutoría ha sido aceptada y agendada con {tutorUser.FullName} para la materia {meeting.Subject.Name}" 
                : $"Su tutoría ha sido rechazada por {tutorUser.FullName}, proceda a elegir otro tutor";

            string subject = meeting.Status == MeetingStatus.Accepted ? "Tutoría Aceptada" : "Tutoría Rechazada";

            var data = new Dictionary<string, string>();
            data.Add("answeredMeetingId", meeting.Id.ToString());
            
            if (meeting.Status != MeetingStatus.Rejected)
            {
                data.Add("meetingRejected", "true");
            }

            await _notificationService.SendNotificationToUser(meeting.Student, message, subject, data);

            return BasicOperationResult<string>.Ok("Notification was sent");
        }
        
        public async Task<IOperationResult<string>> NotifyMeetingAccepted(int userId)
        {

            var userResult = await GetUser(userId);

            if (!userResult.Success) return BasicOperationResult<string>.Fail(userResult.Message.Message);

            var user = userResult.Entity;
            
            await _notificationService.SendNotificationToUser(user,
                "Es para indicarle que la tutoria que acaba de solicitar ha sido aceptada por un tutor",
                "Tutoria Aceptada");
            
            return BasicOperationResult<string>.Ok("La notificacion ha sido enviada");
        }

        private async Task<IOperationResult<User>> GetUser(int userId)
        {
            User user = await _userRepository.Find(u => u.Id == userId, u => u.UserRoles);

            if (user == null)
            {
                return BasicOperationResult<User>.Fail("El usuario no fue encontrado");
            }
            
            return BasicOperationResult<User>.Ok(user);
        }
    }
}