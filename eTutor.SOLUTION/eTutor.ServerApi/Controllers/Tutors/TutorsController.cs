using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Contracts;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/tutors")]
    [ApiController]
    [Authorize]
    public class TutorsController : EtutorBaseController
    {
        private readonly TutorsManager _tutorsManager;
        private readonly RatingManager _ratingManager;
        private readonly IMapper _mapper;

        public TutorsController(TutorsManager tutorsManager, IMapper mapper, RatingManager ratingManager)
        {
            _tutorsManager = tutorsManager;
            _mapper = mapper;
            _ratingManager = ratingManager;
        }


        [HttpGet("inactive")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<UserAdminResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetInactiveTutors()
        {
            var result = await _tutorsManager.GetListOfTutorsFilteredByIsActive(false);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<UserAdminResponse>>(result.Entity);

            return Ok(mapped);
        }


        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<UserAdminResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetActiveTutors()
        {
            var result = await _tutorsManager.GetListOfTutorsFilteredByIsActive();

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<UserAdminResponse>>(result.Entity);

            return Ok(mapped);
        }

        [HttpPost("activate/{tutorId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(UserAdminResponse), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> ActivateTutor([FromRoute] int tutorId)
        {
            var result = await _tutorsManager.ActivateUserForTutor(tutorId);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            var mapped = _mapper.Map<UserAdminResponse>(result.Entity);

            return Ok(mapped);
        }
        
        [HttpPost("inactivate/{tutorId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(UserAdminResponse), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<IActionResult> InactivateTutor([FromRoute] int tutorId)
        {
            var result = await _tutorsManager.ActivateUserForTutor(tutorId, false);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            var mapped = _mapper.Map<UserAdminResponse>(result.Entity);

            return Ok(mapped);
        }

        [HttpGet("{subjectId}/subject")]
        [Authorize(Roles = "admin, student, parent")]
        [ProducesResponseType(typeof(ISet<TutorSimpleResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetTutorsForSubject([FromRoute] int subjectId)
        {
            IOperationResult<ISet<User>> result = await _tutorsManager.GetTutorsBySubjectId(subjectId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var tutors = _mapper.Map<ISet<TutorSimpleResponse>>(result.Entity);

            IEnumerable<TutorSimpleResponse> mapped = tutors.Select(t =>
            {
                t.Ratings = _ratingManager.GetUserAvgRatings(t.Id);
                return t;
            });

            return Ok(mapped);
        }

        [HttpGet("{tutorId}")]
        [Authorize(Roles = "student, parent, admin")]
        [ProducesResponseType(typeof(TutorSimpleResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetTutorById([FromRoute] int tutorId)
        {
            IOperationResult<User> result = await _tutorsManager.GetTutorById(tutorId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var response = _mapper.Map<TutorSimpleResponse>(result.Entity);

            response.Ratings = _ratingManager.GetUserAvgRatings(response.Id);

            return Ok(response);
        }
    }
}