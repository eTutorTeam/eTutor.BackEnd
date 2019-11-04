using eTutor.Core.Models;
using FluentValidation;

namespace eTutor.Core.Validations
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.UserName).NotEmpty();
            RuleFor(u => u.Password).NotEmpty();
        }
    }
}