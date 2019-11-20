using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class TutorSubjectRepository : BaseRepository<TutorSubject>, ITutorSubjectRepository

    {
        public TutorSubjectRepository(ETutorContext context) : base(context)
        {
        }
    }
}
