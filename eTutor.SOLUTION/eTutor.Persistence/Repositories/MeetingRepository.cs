using System;
using System.Collections.Generic;
using System.Text;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class MeetingRepository : BaseRepository<Meeting>, IMeetingRepository
    {
        public MeetingRepository(ETutorContext context) : base(context)
        { }
    }


}
