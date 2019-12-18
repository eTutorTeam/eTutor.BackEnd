using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using FluentValidation;

namespace eTutor.Core.Validations
{
    public sealed class MeetingValidator: AbstractValidator<Meeting>
    {
        public MeetingValidator()
        {
            RuleFor(s => s.TutorId).NotEmpty();

            RuleFor(s => s.StudentId).NotEmpty();

            RuleFor(s => s.SubjectId).NotNull();

            RuleFor(s => s.StartDateTime).NotEmpty();

            RuleFor(s => s.EndDateTime).NotEmpty().GreaterThan(s => s.StartDateTime);

        }
    }
}
