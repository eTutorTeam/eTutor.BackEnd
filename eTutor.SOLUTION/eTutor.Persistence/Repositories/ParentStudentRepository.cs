using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class ParentStudentRepository : BaseRepository<ParentStudent>, IParentStudentRepository
    {
        public ParentStudentRepository(ETutorContext context) : base(context)
        {
        }
    }
}
