using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/tutor-subject")]
    [Authorize]
    [Produces("application/json")]
    public class TutorSubjectController : EtutorBaseController
    {
        private readonly TutorsManager _tutorsManager;
        private readonly SubjectsManager _subjectsManager;
        private readonly TutorSubjectsManager _tutorSubjectsManager;
        private readonly IMapper _mapper;

        public TutorSubjectController(TutorsManager tutorsManager, 
            SubjectsManager subjectsManager, IMapper mapper, TutorSubjectsManager tutorSubjectsManager)
        {
            _tutorsManager = tutorsManager;
            _subjectsManager = subjectsManager;
            _mapper = mapper;
            _tutorSubjectsManager = tutorSubjectsManager;
        }

        [HttpGet("get-subjects/{tutorId}/tutor")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<SubjectResponse>), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> GetSubjectsForTutorAdmin([FromRoute] int tutorId, [FromQuery] bool inverse = false)
        {
            var result = await _subjectsManager.GetSubjectsForTutor(tutorId, inverse);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            var subjects = result.Entity;

            var response = _mapper.Map<IEnumerable<SubjectResponse>>(subjects);

            return Ok(response);
        }
        
        [HttpGet("get-subjects/tutor")]
        [Authorize(Roles = "tutor")]
        [ProducesResponseType(typeof(IEnumerable<SubjectResponse>), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> GetSubjectsForTutor()
        {
            int tutorId = GetUserId();
            
            var result = await _subjectsManager.GetSubjectsForTutor(tutorId);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            var subjects = result.Entity;

            var response = _mapper.Map<IEnumerable<SubjectResponse>>(subjects);

            return Ok(response);
        }

        [HttpPost("assign-subjects/{tutorId}/tutor")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> AssignSubjectsToTutor([FromRoute] int tutorId, [FromBody] TutorSubjectAssignmentModel request)
        {
            var result = await _tutorSubjectsManager.AssignSubjectToTutors(tutorId, request.SubjectIds);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Accepted();
        }

        [HttpGet("get-tutors/subject")]
        [ProducesResponseType(typeof(IEnumerable<SubjectResponse>), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        [AllowAnonymous]
        public async Task<IActionResult> GetTutorsFromSubject([FromHeader] int subjectId)
        {

            var result = await _tutorSubjectsManager.GetTutorsForSubject(subjectId);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            var tutors = result.Entity;
            var response = _mapper.Map<IEnumerable<UserResponse>>(tutors);

            return Ok(response);
        }


    }
}