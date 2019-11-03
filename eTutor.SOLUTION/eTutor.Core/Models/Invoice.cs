using System.Collections.Generic;

namespace eTutor.Core.Models
{
    public sealed class Invoice : EntityBase
    {
        public int MeetingId { get; set; }

        public Meeting Meeting { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public double Amount { get; set; }
        
        public ISet<Payment> Payments { get; set; }
    }
}