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
                .LessThanOrEqualTo(10).WithMessage("Debe de enviar un valor menor o igual a 10")
                .GreaterThanOrEqualTo(0).WithMessage("Debe de enviar un valor igual a 0 o mayor")
                .NotEmpty()
                .WithMessage("El campo Calificacion no puede estar vacío");

            RuleFor(r => r.MeetingId)
                .NotEmpty()
                .WithMessage("El campo Meeting no puede estar vacío");
        }

    }
}
