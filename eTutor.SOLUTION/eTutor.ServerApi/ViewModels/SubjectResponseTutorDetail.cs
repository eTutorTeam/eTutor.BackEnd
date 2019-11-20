using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public class SubjectResponseTutorDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<TutorSimpleResponse> Tutors { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
