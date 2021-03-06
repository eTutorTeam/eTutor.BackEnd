using System;
using Microsoft.AspNetCore.Identity;

namespace eTutor.Core.Models
{
    public sealed class UserClaim : IdentityUserClaim<int>, IEntityBase
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}