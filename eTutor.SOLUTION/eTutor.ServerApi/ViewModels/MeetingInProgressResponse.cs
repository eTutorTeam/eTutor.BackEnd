using System;

namespace eTutor.ServerApi.ViewModels
{
    public class MeetingInProgressResponse
    {
        public int MeetingId { get; set; }
        
        public int StudentId { get; set; }
        
        public string StudentName { get; set; }
        
        public string StudentPhone { get; set; }

        public string StudentImg { get; set; }

        public string SubjectName { get; set; }

        public decimal StudentRatings { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public int TutorId { get; set; }
        
        public string TutorName { get; set; }
        
        public string TutorImg { get; set; }
        
        public string TutorPhone { get; set; }
        
        public decimal TutorRatings { get; set; }
        
        public DateTime RealStartTime { get; set; }
    }
}