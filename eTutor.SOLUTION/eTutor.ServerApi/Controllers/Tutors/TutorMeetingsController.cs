using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.ServerApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("api/tutor-meetings")]
    [Authorize(Roles = "tutor")]
    public class TutorMeetingsController : EtutorBaseController
    {
        private readonly MeetingsManager _meetingsManager;

        public TutorMeetingsController(MeetingsManager meetingsManager)
        {
            _meetingsManager = meetingsManager;
        }

        [HttpPatch("{meetingId}/tutor-answer")]
        [ProducesResponseType(typeof(IOperationResult<string>),202)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> SetTutorMeetingResponseToNotification( [FromRoute] int meetingId,
            [FromBody] MeetingStatusRequest answeredStatus)
        {
            int userId = GetUserId();

            IOperationResult<string> result =
                await _meetingsManager.TutorResponseToMeetingRequest(meetingId, answeredStatus.AnsweredStatus, userId);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Accepted(result);
        }
    }
}