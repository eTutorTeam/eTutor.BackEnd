using System;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using eTutor.Core.Models.Configuration;
using eTutor.Core.Repositories;
using FireBase.Notification;
using FirebaseNoti = FireBase.Notification.Firebase;

namespace eTutor.PushNotificationService
{
    public sealed class PushNotificationService: INotificationService
    {

        private readonly IDeviceRepository _deviceRepository;
        private readonly FirebaseAdminConfiguration _firebaseAdminConfiguration;

        public PushNotificationService(IDeviceRepository deviceRepository, FirebaseAdminConfiguration firebaseAdminConfiguration)
        {
            _deviceRepository = deviceRepository;
            _firebaseAdminConfiguration = firebaseAdminConfiguration;
        }

        public async Task SendNotificationToUser(User user, string message, string subject = "eTutor")
        {
            var devices = await _deviceRepository.FindAll(u => u.UserId == user.Id);
            var deviceTokens = devices.Select(d => d.FcmToken).ToArray();

            using (FirebaseNoti firebase = new FirebaseNoti())
            {
                firebase.ServerKey = _firebaseAdminConfiguration.ServerKey;
                await firebase.PushNotifyAsync(deviceTokens, subject, message);
            }
        }
    }
}