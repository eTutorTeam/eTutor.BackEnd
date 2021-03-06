﻿using System;
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
        private readonly RatingManager _ratingManager;
        private readonly IMapper _mapper;

        public MeetingController( MeetingsManager meetingsManager, IMapper mapper, RatingManager ratingManager)
        {
            _meetingsManager = meetingsManager;
            _mapper = mapper;
            _ratingManager = ratingManager;
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
        [HttpPatch("cancel-meeting/{meetingId}")]
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


        [HttpPatch("start-meeting/{meetingId}")]
        [ProducesResponseType(typeof(MeetingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "tutor")]
        public async Task<IActionResult> StartMeeting([FromRoute] int meetingId)
        {
            int userId = GetUserId();
            IOperationResult<Meeting> result = await _meetingsManager.StartMeeting(meetingId, userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<MeetingResponse>(result.Entity);

            return Ok(mapped);
        }

        [HttpPatch("end-meeting/{meetingId}")]
        [ProducesResponseType(typeof(MeetingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "tutor, student")]
        public async Task<IActionResult> EndMeeting([FromRoute] int meetingId)
        {
            int userId = GetUserId();
            IOperationResult<Meeting> result = await _meetingsManager.EndMeeting(meetingId, userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<MeetingResponse>(result.Entity);

            return Ok(mapped);
        }

        [HttpGet("meeting-in-course")]
        [ProducesResponseType(typeof(IEnumerable<MeetingResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "student, tutor, parent")]
        public async Task<IActionResult> GetCurrentMeeting()
        {
            int userId = GetUserId();

            IOperationResult<Meeting> result = await _meetingsManager.GetCurrentMeeting(userId);

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

        [HttpGet("history")]
        [ProducesResponseType(typeof(ISet<HistoryMeetingReponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "student, tutor, parent")]
        public async Task<IActionResult> GetMeetingsHistoryForUser()
        {
            int userId = GetUserId();
            
            IOperationResult<ISet<Meeting>> result = await _meetingsManager.GetMeetingsHistory(userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<ISet<HistoryMeetingReponse>>(result.Entity);
            
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

            var mapped = _mapper.Map<MeetingResponse>(result.Entity);

            return Ok(mapped);
        }

        [HttpGet("{meetingId}/summary")]
        [ProducesResponseType(typeof(MeetingSummaryModel), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [Authorize(Roles = "tutor, student")]
        public async Task<IActionResult> GetMeetingSummary([FromRoute] int meetingId)
        {
            int userId = GetUserId();

            IOperationResult<Meeting> operationResult =
                await _meetingsManager.GetMeeting(meetingId, userId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            MeetingSummaryModel mapped = _mapper.Map<MeetingSummaryModel>(operationResult.Entity);

            mapped.StudentRatings = _ratingManager.GetUserAvgRatings(mapped.StudentId);
            mapped.TutorRatings = _ratingManager.GetUserAvgRatings(mapped.TutorId);
            
            return Ok(mapped);
        }

        [HttpGet("in-progress")]
        [ProducesResponseType(typeof(MeetingInProgressResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetMeetingInProgress()
        {
            int userId = GetUserId();

            IOperationResult<Meeting> operationResult = await _meetingsManager.GetInProgressMeetingForUser(userId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var mapped = _mapper.Map<MeetingInProgressResponse>(operationResult.Entity);

            mapped.StudentRatings = _ratingManager.GetUserAvgRatings(mapped.StudentId);
            mapped.TutorRatings = _ratingManager.GetUserAvgRatings(mapped.TutorId);
            
            return Ok(mapped);
        }

    }
}
