using System;
using eTutor.Core.Enums;
using eTutor.Core.Models;

namespace eTutor.ServerApi.ViewModels
{
    public class ParentMeetingResponse
    {
        public int Id { get; set; }
        
        public int SubjectId { get; set; }

        public SubjectResponse Subject { get; set; }

        public int StudentId { get; set; }

        public StudentUserViewModel Student { get; set; }

        public int TutorId { get; set; }

        public TutorSimpleResponse Tutor { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public MeetingStatus Status { get; set; }
        
    }
}