using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public class SubjectResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TutorsCount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
