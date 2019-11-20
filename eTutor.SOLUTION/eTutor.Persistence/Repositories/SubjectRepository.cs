using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class SubjectRepository : BaseRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ETutorContext context) : base(context)
        {
        }
    }
}
