using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Models;
using eTutor.Core.Repositories;

namespace eTutor.Core.Managers
{
    public sealed class TutorSubjectsManager
    {
        private readonly ITutorSubjectRepository _tutorSubjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository; 

        public TutorSubjectsManager(ITutorSubjectRepository tutorSubjectRepository, 
            IUserRepository userRepository, ISubjectRepository subjectRepository)
        {
            _tutorSubjectRepository = tutorSubjectRepository;
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<IOperationResult<bool>> AssignSubjectToTutors(int tutorId, IEnumerable<int> subjectIds)
        {
            var tutor = await _userRepository.Find(u => u.Id == tutorId 
                                                        && u.UserRoles.Any(r => r.RoleId == (int)RoleTypes.Tutor),
                                                            u => u.UserRoles);
            if (tutor == null)
            {
                return BasicOperationResult<bool>.Fail("El tutor no fue encontrado");
            }

            bool subjectExists = await CheckIfSubjectIdsExists(subjectIds);

            if (!subjectExists)
            {
                return BasicOperationResult<bool>.Fail("Revise que todas las materias que esta enviando, están registradas en el sistema");
            }

            var tutorSubjects = await GetSubjectsForTutor(tutorId);
            var tutorSubjectIds = tutorSubjects.Select(s => s.Id);
            
            var idOfSubjectsToRemove = GetIdsThatHaveToBeRemoved(tutorSubjectIds, subjectIds);
            var idsOfSubjectsToAdd = GetIdsThatHaveToBeAdded(tutorSubjectIds, subjectIds);

            var tutorSubjecsToRemove = await _tutorSubjectRepository.FindAll(ts =>
                idOfSubjectsToRemove.Contains(ts.SubjectId) && ts.TutorId == tutorId);
            
            _tutorSubjectRepository.Set.RemoveRange(tutorSubjecsToRemove);

            var tutorSubjectsToAdd = idsOfSubjectsToAdd.Select(id => new TutorSubject
            {
                SubjectId = id,
                TutorId = tutorId
            });

            await _tutorSubjectRepository.Set.AddRangeAsync(tutorSubjectsToAdd);
            await _tutorSubjectRepository.Save();
            
            return BasicOperationResult<bool>.Ok(true);
        }
        public async Task<IOperationResult<IEnumerable<User>>> GetTutorsForSubject(int subjectId)
        {
            var tutorSubjects = await _tutorSubjectRepository.FindAll(ts => ts.SubjectId == subjectId, ts => ts.Tutor);
            var tutors = tutorSubjects.Select(ts => ts.Tutor);

            if (!tutors.Any())
            {
                return BasicOperationResult<IEnumerable<User>>.Fail("No hay tutores asociados a esta materia");
            }

            return BasicOperationResult<IEnumerable<User>>.Ok(tutors);
        }
        private async Task<IEnumerable<Subject>> GetSubjectsForTutor(int tutorId)
        {
            var tutorSubjects = await _tutorSubjectRepository.FindAll(ts => ts.TutorId == tutorId, ts => ts.Subject);

            return tutorSubjects.Select(ts => ts.Subject);
        }

        private IEnumerable<int> GetIdsThatHaveToBeRemoved(IEnumerable<int> old, IEnumerable<int> @new)
            => old.Where(n => !@new.Contains(n));

        private IEnumerable<int> GetIdsThatHaveToBeAdded(IEnumerable<int> old, IEnumerable<int> @new)
        {
            var removers = GetIdsThatHaveToBeRemoved(old, @new);
            var numbers = @new.Where(n => !old.Contains(n) && !removers.Contains(n));
            return numbers;
        }

        private async Task<bool> CheckIfSubjectIdsExists(IEnumerable<int> subjectIds)
        {
            foreach (var subjectId in subjectIds)
            {
                if (!await _subjectRepository.Exists(sub => sub.Id == subjectId))
                {
                    return false;
                }
            }

            return true;
        }
    }
}