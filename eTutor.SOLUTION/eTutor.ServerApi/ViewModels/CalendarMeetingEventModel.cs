using System;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class CalendarMeetingEventModel
    {
        public int MeetingId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Title { get; set; }

        public bool allDay = false;

        public TutorSimpleResponse Tutor { get; set; }

        public StudentUserViewModel Student { get; set; }
        
        public SubjectSimpleResponse Subject { get; set; }
    }
}
