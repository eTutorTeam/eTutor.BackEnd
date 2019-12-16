using System;
using System.Collections.Generic;

namespace eTutor.Core.Models
{
    public class ParentAuthorization : EntityBase
    {
        public int ParentId { get; set; }

        public User Parent { get; set; }

        public DateTime AuthorizationDate { get; set; }
        
        public ISet<Meeting> Meetings { get; set; }
    }
}