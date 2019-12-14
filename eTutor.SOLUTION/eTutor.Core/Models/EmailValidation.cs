using System;

namespace eTutor.Core.Models
{
    public sealed class EmailValidation : EntityBase
    {
        public Guid ValidationToken { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}