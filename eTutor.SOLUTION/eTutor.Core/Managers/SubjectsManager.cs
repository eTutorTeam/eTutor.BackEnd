using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
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

        public SubjectsManager(ISubjectRepository subjectRepository, ITutorSubjectRepository tutorSubjectRepository)
        {
            _subjectRepository = subjectRepository;
            _tutorSubjectRepository = tutorSubjectRepository;
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
    }
}
