using System;
using eTutor.Core.Enums;

namespace eTutor.ServerApi.ViewModels
{
    public class HistoryMeetingReponse
    {
        public int Id { get; set; }

        public string Subject { get; set; }
        
        public string TutorName { get; set; }
        
        public string TutorImg { get; set; }
        
        public string StudentName { get; set; }
        
        public string StudentImg { get; set; }
        
        public DateTime ScheduledDate { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public MeetingStatus Status { get; set; }
    }
}