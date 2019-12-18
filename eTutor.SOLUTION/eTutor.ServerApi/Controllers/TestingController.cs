using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Managers;
using eTutor.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTutor.ServerApi.Controllers
{
    [Route("test-api")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class TestingController : EtutorBaseController
    {

        private NotificationManager _notificationManager;

        public TestingController(NotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        [HttpGet("test-notification")]
        public async Task<IActionResult> TestSendingNotifications([FromQuery] int userId = 5)
        {
            IOperationResult<string> result = await _notificationManager.NotifyMeetingAccepted(userId);

            return Ok(result);
        }
    }
}