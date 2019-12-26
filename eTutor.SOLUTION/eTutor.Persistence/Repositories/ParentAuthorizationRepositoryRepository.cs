using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Persistence.Repositories
{
    public class ParentAuthorizationRepositoryRepository : BaseRepository<ParentAuthorization>, IParentAuthorizationRepository
    {
        public ParentAuthorizationRepositoryRepository(ETutorContext context) : base(context)
        {
        }
    }
}