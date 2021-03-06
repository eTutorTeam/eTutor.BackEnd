﻿using System;
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

namespace eTutor.Core.Managers
{
    public sealed class RatingManager
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMeetingRepository _meetingRepository;
        private readonly IUserRepository _userRepository;
        public RatingManager(IRatingRepository ratingRepository, IUserRepository userRepository, IMeetingRepository meetingRepository)
        {
            _ratingRepository = ratingRepository;
            _userRepository = userRepository;
            _meetingRepository = meetingRepository;
        }

        public async Task<IOperationResult<IEnumerable<Rating>>> GetUserRatings(int userId)
        {
            var rating = await _ratingRepository.FindAll(r => r.UserId == userId);
            
            if (rating == null)
            {
                return BasicOperationResult<IEnumerable<Rating>>.Fail("Los ratings no están disponibles");
            }

            return BasicOperationResult<IEnumerable<Rating>>.Ok(rating);
        }

        public async Task<IOperationResult<decimal>> GetUserAvgRatingAsync(int userId)
        {
            var ratings = await _ratingRepository.FindAll(r => r.UserId == userId);

            if (!ratings.Any())
            {
                return BasicOperationResult<decimal>.Fail("El rating no esta disponible");
            }

            decimal avgRating = ratings.Sum(r => r.Calification) / ratings.Count() / 2;

            return BasicOperationResult<decimal>.Ok(avgRating);
        }

        public decimal GetUserAvgRatings(int userId)
        {
            IOperationResult<decimal> result = GetUserAvgRatingAsync(userId).Result;

            Task.WaitAll();

            if (!result.Success)
            {
                return 5;
            }
            
            return result.Entity;
        }

        public async Task<IOperationResult<Rating>> CreateUserRating(Rating rating)
        {
            if (rating == null)
            {
                return BasicOperationResult<Rating>.Fail("El objeto enviado es inváalido");
            }

            var validation = await ValidateRating(rating);
            if (!validation.Success) return BasicOperationResult<Rating>.Fail("Objeto Invalido");

            _ratingRepository.Create(rating);

            await _ratingRepository.Save();

            var response = await _ratingRepository.Find(r => r.Id == rating.Id, r => r.Meeting, r => r.User);

            return BasicOperationResult<Rating>.Ok(response);
        }

        private async Task<IOperationResult<Rating>> ValidateRating(Rating rating)
        {
            var validator = new RatingValidator();

            ValidationResult validationResult = await validator.ValidateAsync(rating);

            if (!validationResult.IsValid)
            {
                return BasicOperationResult<Rating>.Fail(validationResult.JSONFormatErrors());
            }

            return BasicOperationResult<Rating>.Ok();
        }

        public async Task<IOperationResult<Meeting>> GetMeetingPendingForRatingForUser(int userId)
        {
            var userExists = await _userRepository.Exists(u => u.Id == userId);

            if (!userExists)
            {
                return BasicOperationResult<Meeting>.Fail("El usuario no fue encontrado");
            }

            RoleTypes userRole = (await _userRepository.GetRolesForUser(userId)).FirstOrDefault();
            
            Meeting lastMeeting = await _meetingRepository.GetLastCompleteMeetingForUser(userId, userRole);

            bool ratingExists = await _ratingRepository.Exists(r => r.UserId == userId && r.MeetingId == lastMeeting.Id);

            if (!ratingExists)
            {
                return BasicOperationResult<Meeting>.Ok(lastMeeting);
            }
            
            return BasicOperationResult<Meeting>.Ok(null);
        }
    }
}
