using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class ChangePasswordRepository : BaseRepository<ChangePassword>, IChangePasswordRepository
    {
        public ChangePasswordRepository(ETutorContext context) : base(context)
        {
        }
    }
}
