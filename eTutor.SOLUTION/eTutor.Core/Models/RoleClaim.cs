using Microsoft.AspNetCore.Identity;

namespace eTutor.Core.Models
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public override int RoleId { get; set; }
        
        public Role Role { get; set; }
    }
}