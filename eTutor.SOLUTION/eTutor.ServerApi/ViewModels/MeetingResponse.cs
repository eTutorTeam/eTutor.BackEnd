using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Enums;

namespace eTutor.ServerApi.ViewModels
{
    public class MeetingResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        public int TutorId { get; set; }

        public int SubjectId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public MeetingStatus Status { get; set; }
    }
}
