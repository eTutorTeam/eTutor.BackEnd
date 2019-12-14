using eTutor.Core.Models;
using FluentValidation;

namespace eTutor.Core.Validations
{
    public sealed class DevicesValidator : AbstractValidator<Device>
    {
        public DevicesValidator()
        {
            RuleFor(d => d.Platform)
                .NotEmpty()
                .WithMessage("El campo de plataforma no puede estar vacío");
            
            RuleFor(d => d.UserId)
                .NotEmpty()
                .WithMessage("El ID del usuario es un campo requerido");

            RuleFor(d => d.FcmToken)
                .NotEmpty()
                .WithMessage("El token del dispostivo es requerido");
        }
    }
}