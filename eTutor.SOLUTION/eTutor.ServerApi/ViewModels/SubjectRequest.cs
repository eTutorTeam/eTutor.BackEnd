using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTutor.ServerApi.ViewModels
{
    public sealed class SubjectRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
