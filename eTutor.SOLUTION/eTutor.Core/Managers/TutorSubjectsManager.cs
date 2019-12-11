using eTutor.Core.Repositories;

namespace eTutor.Core.Managers
{
    public sealed class TutorSubjectsManager
    {
        private readonly ITutorSubjectRepository _tutorSubjectRepository;

        public TutorSubjectsManager(ITutorSubjectRepository tutorSubjectRepository)
        {
            _tutorSubjectRepository = tutorSubjectRepository;
        }
    }
}