using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public sealed class RejectedMeetingRepository : BaseRepository<RejectedMeeting>, IRejectedMeetingRepository
    {
        public RejectedMeetingRepository(ETutorContext context) : base(context)
        {
        }
    }
}