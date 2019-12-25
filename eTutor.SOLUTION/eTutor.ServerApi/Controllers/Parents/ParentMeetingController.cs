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
    [Route("parent-meeting")]
    [Authorize(Roles = "parent")]
    [Produces("application/json")]
    public class ParentMeetingController : EtutorBaseController
    {
        private readonly MeetingsManager _meetingsManager;
        private readonly IMapper _mapper;
        private readonly ParentAuthorizationManager _parentAuthorizationManager;

        public ParentMeetingController(MeetingsManager meetingsManager, 
            ParentAuthorizationManager parentAuthorizationManager)
        {
            _meetingsManager = meetingsManager;
            _parentAuthorizationManager = parentAuthorizationManager;
        }

        [HttpGet("pending")]
        [ProducesResponseType(typeof(IEnumerable<ParentMeetingResponse>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetMeetingsPendingApprove()
        {
            int parentId = GetUserId();

            var result = await _meetingsManager.GetFutureParentMeetings(parentId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var mapped = _mapper.Map<IEnumerable<ParentMeetingResponse>>(result.Entity);

            return Ok(mapped);
        }
    }
}