using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Models;
using eTutor.Core.Models.Configuration;
using eTutor.Core.Repositories;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace eTutor.PushNotificationService
{
    public sealed class PushNotificationService: INotificationService
    {

        private readonly IDeviceRepository _deviceRepository;
        private readonly FirebaseMessaging _firebaseMessaging;
        

        public PushNotificationService(IDeviceRepository deviceRepository, AppBaseRoute route)
        {
            _deviceRepository = deviceRepository;

            string jsonPath = Path.Combine(route.BasePath, "etutorfirebaseadmin.json");

            var configurationJson = File.ReadAllText(jsonPath);

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(configurationJson)
                });
            }

            _firebaseMessaging = FirebaseMessaging.DefaultInstance;

        }

        public async Task SendNotificationToUser(User user, string message, string subject = "eTutor")
        {
            var devices = await _deviceRepository.FindAll(u => u.UserId == user.Id);
            var deviceTokens = devices.Select(d => d.FcmToken).ToArray();

            var multicastMessage = new MulticastMessage
            {
                Tokens = deviceTokens,
                Notification = new Notification
                {
                    Body = message,
                    Title = subject
                }
            };

            await _firebaseMessaging .SendMulticastAsync(multicastMessage);
        }
        
    }
}