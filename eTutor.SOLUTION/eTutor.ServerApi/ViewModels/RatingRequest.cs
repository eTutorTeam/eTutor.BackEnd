using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public class RatingRequest
    {
        public int UserId { get; set; }
        public int MeetingId { get; set; }

        [Range(0, 10)]
        public int Calification { get; set; }
    }
}
