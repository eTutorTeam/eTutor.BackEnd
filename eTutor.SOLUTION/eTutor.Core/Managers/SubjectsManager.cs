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

        public SubjectsManager(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
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
            var validator = new SubjectValidator();

            ValidationResult validationResult = await validator.ValidateAsync(subject);

            if (!validationResult.IsValid)
            {
                return BasicOperationResult<Subject>.Fail(validationResult.JSONFormatErrors());
            }

           var res = _subjectRepository.Create(subject);

           if (!res.Success) return res;

           await _subjectRepository.Save();

           return BasicOperationResult<Subject>.Ok(res.Entity);
        }
    }
}
