using System;
using System.Collections.Generic;
using eTutor.Core.Enums;

namespace eTutor.Core.Models
{
    public sealed class Meeting : EntityBase
    {
        public int SubjectId { get; set; }

        public Subject Subject { get; set; }

        public int StudentId { get; set; }

        public User Student { get; set; }

        public int TutorId { get; set; }

        public User Tutor { get; set; }

        public int? ParentAuthorizationId { get; set; }

        public ParentAuthorization ParentAuthorization { get; set; }
        
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public MeetingStatus Status { get; set; }
        
        public ISet<Rating> Ratings { get; set; }
        
        public ISet<Invoice> Invoices { get; set; }

        public int? CancelerUserId { get; set; }

        public User CancelerUser { get; set; }

        public DateTime RealStartedDateTime { get; set; }
    }
}