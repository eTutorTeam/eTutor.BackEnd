using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class MeetingRequest
    {
        public int Id { get; set; }

        public int TutorId { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
