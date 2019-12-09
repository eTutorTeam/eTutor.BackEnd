using System;
using System.Collections.Generic;
using System.Text;

namespace eTutor.Core.Models
{
    public sealed class ChangePassword : EntityBase
    {
         public Guid ChangeRequestId { get; set; }
         
         public int UserId { get; set; }
         
         public bool IsUsed { get; set; }
         
        public User User { get; set; }
        
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(1);
    }
}
