using System;
namespace eTutor.Core.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime GetNowInCorrectTimezone(this DateTime date)
        {
            date = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, "SA Western Standard Time");
        }
    }
}
