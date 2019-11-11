using System;
using System.Collections.Generic;
using eTutor.Core.Enums;

namespace eTutor.Core.Models
{
    public sealed class Meeting : EntityBase
    {
        public int TopicId { get; set; }

        public Topic Topic { get; set; }

        public int StudentId { get; set; }

        public User Student { get; set; }

        public int TutorId { get; set; }

        public User Tutor { get; set; }

        public int? ParentAutorizationId { get; set; }

        public ParentAutorization Type { get; set; }
        
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public MeetingStatus Status { get; set; }
        
        public ISet<Rating> Ratings { get; set; }
        
        public ISet<Invoice> Invoices { get; set; }
    }
}