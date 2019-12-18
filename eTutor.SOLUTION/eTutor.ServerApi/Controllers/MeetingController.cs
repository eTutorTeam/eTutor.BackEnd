using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using eTutor.Core.Contracts;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/meetings")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class MeetingController: EtutorBaseController
    {
        private readonly MeetingsManager _meetingsManager;
        private readonly IMapper _mapper;

        public MeetingController( MeetingsManager meetingsManager, IMapper mapper)
        {
            _meetingsManager = meetingsManager;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(MeetingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> CreateMeeting([FromBody] MeetingRequest request)
        {
            Meeting model = _mapper.Map<Meeting>(request);

            IOperationResult<Meeting> operationResult = await _meetingsManager.CreateMeeting(model);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var response = _mapper.Map<SubjectResponse>(operationResult.Entity);

            return Ok(response);
        }

        [HttpGet("student-meetings")]
        [ProducesResponseType(typeof(IEnumerable<MeetingResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetStudentMeetings()
        {
            int userId = GetUserId();
            
            IOperationResult<IEnumerable<Meeting>> result = await _meetingsManager.GetStudentMeetings(userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<MeetingResponse>>(result.Entity);

            return Ok(mapped);
        }

        [HttpGet("tutor-meetings")]
        [ProducesResponseType(typeof(IEnumerable<MeetingResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetTutorMeetings()
        {
            int userId = GetUserId();
            
            var result = await _meetingsManager.GetTutorMeetings(userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<MeetingResponse>>(result.Entity);

            return Ok(mapped);
        }


    }
}
