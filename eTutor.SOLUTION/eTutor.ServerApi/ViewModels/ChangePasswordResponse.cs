using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class ChangePasswordResponse
    {
        public Guid ChangeRequestId { get; set; }
        public int UserId { get; set; }
        public UserResponse User { get; set; }
        public DateTime ExpirationDate { get; set; } 
    }
}
