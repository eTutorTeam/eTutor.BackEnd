﻿using System;
using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eTutor.PushNotificationService
{
    public sealed class PushNotificationService: INotificationService
    {

        private readonly IDeviceRepository _deviceRepository;
        private readonly FirebaseMessaging _firebaseMessaging;
        

        public PushNotificationService(IDeviceRepository deviceRepository, FirebaseMessaging firebaseMessaging)
        {
            _deviceRepository = deviceRepository;
            _firebaseMessaging = firebaseMessaging;

        }

        public async Task SendNotificationToUser(User user, string message, string subject = "eTutor", Dictionary<string, string> data = null)
        {
            var devices = await _deviceRepository.FindAll(u => u.UserId == user.Id);
            var deviceTokens = devices.Select(d => d.FcmToken).Distinct().ToArray();

            if (deviceTokens.Length > 0)
            {

                var multicastMessage = new MulticastMessage
                {
                    Tokens = deviceTokens,
                    Notification = new Notification
                    {
                        Body = message,
                        Title = subject
                    },
                    Data = data
                };

                await _firebaseMessaging.SendMulticastAsync(multicastMessage);
            }
        }

        public async Task SendNotificationToMultipleUsers(ISet<User> user, string message, string subject = "eTutor", Dictionary<string, string> data = null)
        {
            string[] deviceTokens = await _deviceRepository
                .Set.Where(d => user.Any(u => u.Id == d.UserId))
                .Select(d => d.FcmToken)
                .Distinct()
                .ToArrayAsync();
            
            if (deviceTokens.Length > 0)
            {

                var multicastMessage = new MulticastMessage
                {
                    Tokens = deviceTokens,
                    Notification = new Notification
                    {
                        Body = message,
                        Title = subject
                    },
                    Data = data
                };

                await _firebaseMessaging.SendMulticastAsync(multicastMessage);
            }
        }
    }
}