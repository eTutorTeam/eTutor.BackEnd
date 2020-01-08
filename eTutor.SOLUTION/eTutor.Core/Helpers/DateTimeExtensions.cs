using System;
namespace eTutor.Core.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime GetNowInCorrectTimezone(this DateTime date)
        {
            return date.AddHours(-4);
        }
    }
}
