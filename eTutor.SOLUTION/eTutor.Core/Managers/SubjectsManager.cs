using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Enums;
using eTutor.Core.Helpers;
using eTutor.Core.Models;
using eTutor.Core.Repositories;
using eTutor.Core.Validations;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace eTutor.Core.Managers
{
    public sealed class SubjectsManager
    {

        private readonly ISubjectRepository _subjectRepository;
        private readonly ITutorSubjectRepository _tutorSubjectRepository;
        private readonly IUserRepository _userRepository;

        public SubjectsManager(ISubjectRepository subjectRepository,
            ITutorSubjectRepository tutorSubjectRepository,
            IUserRepository userRepository)
        {
            _subjectRepository = subjectRepository;
            _tutorSubjectRepository = tutorSubjectRepository;
            _userRepository = userRepository;
        }

        public async Task<IOperationResult<IEnumerable<Subject>>> GetAllSubjects()
        {
            var subjects = await _subjectRepository.Set.Include(s => s.Tutors).ToListAsync();

            return BasicOperationResult<IEnumerable<Subject>>.Ok(subjects);
        }

        public async Task<IOperationResult<Subject>> GetSubject(int subjectId)
        {
            var subject = await _subjectRepository.Find(s => s.Id == subjectId, s => s.Tutors);

            if (subject == null)
            {
                return BasicOperationResult<Subject>.Fail("The subject was not found");
            }

            return BasicOperationResult<Subject>.Ok(subject);
        }

        public async Task<IOperationResult<Subject>> CreateSubject(Subject subject)
        {
            var validation = await ValidateSubject(subject);

            if (!validation.Success) return validation;

            var res = _subjectRepository.Create(subject);

            if (!res.Success) return res;

            await _subjectRepository.Save();

            return BasicOperationResult<Subject>.Ok(res.Entity);
        }

        public async Task<IOperationResult<Subject>> UpdateSubject(Subject subject)
        {
            Subject currentSubject = await _subjectRepository.Set.FindAsync(subject.Id);

            if (currentSubject == null) return BasicOperationResult<Subject>.Fail("La materia dada no fue encontrada");

            var validationResult = await ValidateSubject(subject);
            if (!validationResult.Success) return validationResult;

            _subjectRepository.Update(subject);
            await _subjectRepository.Save();

            return BasicOperationResult<Subject>.Ok(subject);
        }


        public async Task<IOperationResult<Subject>> RemoveSubject(int subjectId)
        {
            Subject subject = await _subjectRepository.Set.FindAsync(subjectId);

            if (subject == null) return BasicOperationResult<Subject>.Fail("Subject not found");

            var tutorRelations = await _tutorSubjectRepository.FindAll(ts => ts.SubjectId == subjectId);
            _tutorSubjectRepository.Set.RemoveRange(tutorRelations);
            await _tutorSubjectRepository.Save();

            _subjectRepository.Remove(subject);
            await _subjectRepository.Save();

            return BasicOperationResult<Subject>.Ok(subject);
        }

        private async Task<IOperationResult<Subject>> ValidateSubject(Subject subject)
        {
            var validator = new SubjectValidator();

            ValidationResult validationResult = await validator.ValidateAsync(subject);

            if (!validationResult.IsValid)
            {
                return BasicOperationResult<Subject>.Fail(validationResult.JSONFormatErrors());
            }

            return BasicOperationResult<Subject>.Ok();
        }

        public async Task<IOperationResult<IEnumerable<Subject>>> GetSubjectsForTutor(int tutorId, bool inverse = false)
        {
            var user = await _userRepository.Find(t => t.Id == tutorId
                                                 && t.UserRoles.Any(r => r.RoleId == (int) RoleTypes.Tutor),
                t => t.UserRoles);

            if (user == null)
            {
                return BasicOperationResult<IEnumerable<Subject>>.Fail("El tutor no fue encontrado");
            }

            if (inverse)
            {
                var inverseSubjects = await GetAllSubjectsAGivenTutorDoesntHave(tutorId);
                    return BasicOperationResult<IEnumerable<Subject>>.Ok(inverseSubjects);
            }
            
            var tutorSubjects = await _tutorSubjectRepository.FindAll(ts => ts.TutorId == tutorId, ts => ts.Subject);
            var subjects = tutorSubjects.Select(t => t.Subject);
            return BasicOperationResult<IEnumerable<Subject>>.Ok(subjects);
        }

        private async Task<IEnumerable<Subject>> GetAllSubjectsAGivenTutorDoesntHave(int tutorId)
        {
            var subjects = _subjectRepository.Set.Include(s => s.Tutors);

            var res = await subjects.Where(s => !s.Tutors.Select(t => t.TutorId).Contains(tutorId)).ToListAsync();

            return res;
        }
    }
}
