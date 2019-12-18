using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
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
                {"subjectName", subject.Name},
                {"subjectId", subject.Id.ToString()},
                {"studentName", student.FullName},
                {"studentId", student.Id.ToString()},
                {"meetingId", meetingId.ToString()}
            };

            await _notificationService.SendNotificationToUser(user, message, $"Tutoria Solicitada: {subject.Name}", data);
            
            return BasicOperationResult<string>.Ok("Tutoria Creada");
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
            var user = await _userRepository.Find(u => u.Id == userId);

            if (user == null)
            {
                return BasicOperationResult<User>.Fail("El usuario no fue encontrado");
            }
            
            return BasicOperationResult<User>.Ok(user);
        }
    }
}