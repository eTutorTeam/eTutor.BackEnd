using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

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

        public async Task<IOperationResult<string>> NotifyMeetingAccepted(int userId)
        {
            var user = await _userRepository.Find(u => u.Id == userId);

            if (user == null)
            {
                return BasicOperationResult<string>.Fail("El usuario no fue encontrado");
            }

            await _notificationService.SendNotificationToUser(user,
                "Es para indicarle que la tutoria que acaba de solicitar ha sido aceptada por un tutor",
                "Tutoria Aceptada");
            
            return BasicOperationResult<string>.Ok("La notificacion ha sido enviada");
        }
    }
}