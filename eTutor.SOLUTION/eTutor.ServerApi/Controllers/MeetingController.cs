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
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

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
        [ProducesResponseType(401)]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> CreateMeeting([FromBody] MeetingStudentRequest studentRequest)
        {
            int studentId = GetUserId();
            Meeting model = _mapper.Map<Meeting>(studentRequest);
            model.StudentId = studentId;

            IOperationResult<Meeting> operationResult = await _meetingsManager.CreateMeeting(model);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var response = _mapper.Map<MeetingResponse>(operationResult.Entity);

            return Ok(response);
        }
        [HttpPut("cancel-meeting/{meetingId}")]
        [ProducesResponseType(typeof(MeetingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> CancelMeeting([FromRoute] int meetingId)
        {
            int userId = GetUserId();
            IOperationResult<Meeting> result = await _meetingsManager.CancelMeeting(meetingId, userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<MeetingResponse>(result.Entity);

            return Ok(mapped);
        }

        [HttpGet("calendar")]
        [ProducesResponseType(typeof(ISet<CalendarMeetingEventModel>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "student, tutor, parent")]
        public async Task<IActionResult> GetMeetingsDependingOnUserRoleForCalendar()
        {
            int userId = GetUserId();

            IOperationResult<ISet<Meeting>> operationResult = await _meetingsManager.GetMeetingsForUserCalendar(userId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var mapped = _mapper.Map<ISet<CalendarMeetingEventModel>>(operationResult.Entity);

            return Ok(mapped);
        }
        

        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<MeetingResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "student, tutor, parent")]
        public async Task<IActionResult> GetStudentTutorMeetings()
        {
            int userId = GetUserId();
            
            IOperationResult<IEnumerable<Meeting>> result = await _meetingsManager.GetStudentTutorMeetings(userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<MeetingResponse>>(result.Entity);

            return Ok(mapped);
        }
        
        [HttpGet("{meetingId}")]
        [ProducesResponseType(typeof(MeetingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetMeeting([FromRoute] int meetingId)
        {
            int userId = GetUserId();
            
            var result = await _meetingsManager.GetMeeting(meetingId, userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<MeetingResponse>>(result.Entity);

            return Ok(mapped);
        }

        [HttpGet("{meetingId}/summary")]
        [ProducesResponseType(typeof(MeetingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "tutor, student")]
        public async Task<IActionResult> GetTutorMeetingSummary([FromRoute] int meetingId)
        {
            int userId = GetUserId();

            IOperationResult<Meeting> operationResult =
                await _meetingsManager.GetMeeting(meetingId, userId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            MeetingResponse mapped = _mapper.Map<MeetingResponse>(operationResult.Entity);

            return Ok(mapped);
        }

    }
}
