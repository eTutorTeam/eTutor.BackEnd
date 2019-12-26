using System;
using System.Collections.Generic;
using eTutor.Core.Enums;

namespace eTutor.Core.Models
{
    public class ParentAuthorization : EntityBase
    {
        public int ParentId { get; set; }
        
        public ParentAuthorizationStatus Status { get; set; }
        
        public string Reason { get; set; }
        
        public User Parent { get; set; }

        public DateTime AuthorizationDate { get; set; }
        
        public ISet<Meeting> Meetings { get; set; }
    }
}