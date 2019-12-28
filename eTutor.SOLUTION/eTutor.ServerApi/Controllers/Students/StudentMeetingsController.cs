using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Contracts;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;

namespace eTutor.ServerApi.Controllers.Students
{
    [Route("api/student-meetings")]
    [Produces("application/json")]
    [Authorize(Roles = "student")]
    public class StudentMeetingsController : EtutorBaseController
    {
        private readonly MeetingsManager _meetingsManager;
        private readonly TutorsManager _tutorsManager;
        private readonly IMapper _mapper;

        public StudentMeetingsController(MeetingsManager meetingsManager, IMapper mapper)
        {
            _meetingsManager = meetingsManager;
            _mapper = mapper;
        }

        [HttpGet("{meetingId}/not-selected-tutors")]
        [ProducesResponseType(typeof(ISet<TutorSimpleResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetNotSelectedTutorsInMeetingForStudent([FromRoute] int meetingId)
        {
            IOperationResult<ISet<User>> operationResult = await _tutorsManager.GetTutorNotInCurMeeting(meetingId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            ISet<TutorSimpleResponse> tutors =
                _mapper.Map<ISet<TutorSimpleResponse>>(operationResult.Entity);

            return Ok(tutors);
        }


        [HttpGet("{meetingId}/random-tutor")]
        [ProducesResponseType(typeof(TutorSimpleResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetRandomTutorForMeeting([FromRoute] int meetingId)
        {
            IOperationResult<User> operationResult = await _tutorsManager.GetRandomNotUsedTutorForMeeting(meetingId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var mapped = _mapper.Map<TutorSimpleResponse>(operationResult.Entity);

            return Ok(mapped);
        }
        

        [HttpPatch("reschedule-tutor")]
        [ProducesResponseType(typeof(MeetingResponse), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> UpdateMeetingWithNewSelectedTutor(
            [FromBody] StudentMeetingTutorReschedule request)
        {
            int studentId = GetUserId();

            IOperationResult<Meeting> operationResult =
                await _meetingsManager.RescheduleTutorForStudentMeeting(request.MeetingId, request.TutorId, studentId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Message);
            }

            var mapped = _mapper.Map<MeetingResponse>(operationResult.Entity);

            return Ok(mapped);
        }
        
        
    }
}