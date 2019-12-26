using eTutor.Core.Repositories;

namespace eTutor.Core.Managers
{
    public class ParentAuthorizationManager
    {
        private IParentAuthorizationRepository _parentAuthorizationRepository;

        public ParentAuthorizationManager(IParentAuthorizationRepository parentAuthorizationRepository)
        {
            _parentAuthorizationRepository = parentAuthorizationRepository;
        }
    }
}