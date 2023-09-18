using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Common
{
    public static class DateTimeExtensions
    {
        public static DateTime CurrentWeekMonday(this DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Monday) return date.Date;
            return date.Date.AddDays((-1) * ((date.DayOfWeek - DayOfWeek.Monday + 7) % 7)); // go back to Monday of this week
        }

        public static DateTime NextWeekMonday(this DateTime date)
        {
            if(date.DayOfWeek == DayOfWeek.Monday) return date.Date.AddDays(7);
            return date.Date.AddDays((DayOfWeek.Monday - date.DayOfWeek + 7) % 7); // go forward to Monday of next week
        }

        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime EndDateTimeOfMonth(this DateTime date)
        {
            return date.FirstDayOfNextMonth().EndOfPrevDateTime();
        }
            
        public static DateTime FirstDayOfNextMonth(this DateTime date)
        {
            return date.FirstDayOfMonth().AddMonths(1);
        }
        public static DateTime EndOfPrevDateTime(this DateTime date)
        {
            return date.AddMinutes(-1);
        }
    }
}
