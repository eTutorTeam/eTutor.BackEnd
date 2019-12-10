using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public TutorsController(TutorsManager tutorsManager, IMapper mapper)
        {
            _tutorsManager = tutorsManager;
            _mapper = mapper;
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
    }
}