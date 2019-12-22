using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace eTutor.Core.Validations
{
    public sealed class RatingValidator : AbstractValidator<Rating>
    {
        public RatingValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage("El campo usuario no puede estar vacío");

            RuleFor(r => r.Calification)
                .NotEmpty()
                .WithMessage("El campo Calificacion no puede estar vacío");

            RuleFor(r => r.MeetingId)
                .NotEmpty()
                .WithMessage("El campo Meeting no puede estar vacío");
        }

    }
}
