using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Helpers;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using eTutor.Core.Validations;
using FluentValidation.Results;

namespace eTutor.Core.Managers
{
    public sealed class RatingManager
    {
        public readonly IRatingRepository _ratingRepository;


        public RatingManager(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<IOperationResult<IEnumerable<Rating>>> getUserRatings(int userId)
        {
            var rating = await _ratingRepository.FindAll(r => r.UserId == userId);

            if (rating == null)
            {
                return BasicOperationResult<IEnumerable<Rating>>.Fail("El rating no esta disponible");
            }

            return BasicOperationResult<IEnumerable<Rating>>.Ok(rating);
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
    }
}
