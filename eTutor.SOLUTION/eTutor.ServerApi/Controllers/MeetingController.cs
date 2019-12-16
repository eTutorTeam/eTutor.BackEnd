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
    [Route("api/accounts")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class MeetingController: EtutorBaseController
    {
        private readonly UsersManager _usersManager;
        private readonly MeetingsManager _meetingsManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MeetingController(UsersManager usersManager, MeetingsManager meetingsManager,IConfiguration configuration ,IMapper mapper)
        {
            _usersManager = usersManager;
            _meetingsManager = meetingsManager;
            _configuration = configuration;
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
        [ProducesResponseType(typeof(IEnumerable<Meeting>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetStudentMeetings([FromBody] int userId)
        {
            var result = await _meetingsManager.GetStudentMeetings(userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<Meeting>>(result.Entity);

            return Ok(mapped);
        }

        [HttpGet("tutor-meetings")]
        [ProducesResponseType(typeof(IEnumerable<Meeting>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetTutorMeetings([FromBody] int userId)
        {
            var result = await _meetingsManager.GetTutorMeetings(userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<Meeting>>(result.Entity);

            return Ok(mapped);
        }


    }
}
