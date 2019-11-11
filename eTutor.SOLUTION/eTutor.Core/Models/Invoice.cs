using System.Collections.Generic;

namespace eTutor.Core.Models
{
    public sealed class Invoice : EntityBase
    {
        public int MeetingId { get; set; }

        public Meeting Meeting { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public double Amount { get; set; }
        
        public ISet<Payment> Payments { get; set; }
    }
}