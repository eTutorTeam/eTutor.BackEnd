using System;
using System.Collections.Generic;
using eTutor.Core.Enums;
using eTutor.Core.Models;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class ParentAuthorizationResponse
    {
        public int Id { get; set; }
        
        public int ParentId { get; set; }
        
        public string Reason { get; set; }
        
        public ParentAuthorizationStatus Status { get; set; }

        public DateTime AuthorizationDate { get; set; }
    }
}