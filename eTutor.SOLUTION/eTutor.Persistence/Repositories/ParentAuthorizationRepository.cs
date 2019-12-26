using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public class ParentAuthorizationRepository : BaseRepository<ParentAuthorization>, IParentAuthorizationRepository
    {
        public ParentAuthorizationRepository(ETutorContext context) : base(context)
        {
        }
    }
}