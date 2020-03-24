using System;
using System.Threading.Tasks;
using eTutor.Core.Contracts;
using eTutor.Core.Helpers;
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

        [HttpGet("demo-date")]
        public IActionResult GetTestDate()
        {
            var date = DateTime.Now;
            var funcDate = date.GetNowInCorrectTimezone();
            var timezone = TimeZone.CurrentTimeZone;
            var strDate = date.ToLongDateString();
            var strTime = date.ToLongTimeString();
            var strTimezone = timezone.StandardName;

            return Ok(new {
                normal = funcDate.ToLongDateString() + "   " + funcDate.ToLongTimeString() ,
                strDate = strDate,
                strTime = strTime,
                strTimezone = strTimezone
            });
        }
    }
}