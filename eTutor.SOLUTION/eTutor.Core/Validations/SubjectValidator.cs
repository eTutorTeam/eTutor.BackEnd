using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using FluentValidation;

namespace eTutor.Core.Validations
{
    public sealed class SubjectValidator : AbstractValidator<Subject>
    {
        public SubjectValidator()
        {
            RuleFor(s => s.Name).NotEmpty().MinimumLength(2).MaximumLength(150);

            RuleFor(s => s.Description).NotEmpty().MinimumLength(2).MaximumLength(1500);
        }
    }
}
