using System;
using Microsoft.AspNetCore.Identity;

namespace eTutor.Core.Models
{
    public sealed class UserClaim : IdentityUserClaim<int>, IEntityBase
    {
        public override int UserId { get; set; }
        
        public User User { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}