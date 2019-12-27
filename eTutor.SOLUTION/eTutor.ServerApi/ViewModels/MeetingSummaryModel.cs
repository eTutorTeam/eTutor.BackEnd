using System;

namespace eTutor.ServerApi.ViewModels
{
    public class MeetingSummaryModel
    {
        public int MeetingId { get; set; }
        
        public string StudentName { get; set; }
        
        public string StudentImg { get; set; }
        
        public string SubjectName { get; set; }
        
        public DateTime MeetingDate { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public string LocationSummary { get; set; }
        
        public float Longitude { get; set; }
        
        public float Latitude { get; set; }
    }
}
