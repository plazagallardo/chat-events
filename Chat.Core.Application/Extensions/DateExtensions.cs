using Chat.Core.Application.Domain.Enums;
using System.Globalization;

namespace Chat.Core.Application.Extensions
{
    public static class DateExtensions
    {
        public static string ToTimeViewString(this DateTime dt, TimeGranularity timeGranularity)
        {
            switch (timeGranularity)
            {
                case (TimeGranularity.Seconds):
                    return TruncateToSecondStart(dt).ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
                case (TimeGranularity.Minutes):
                    return TruncateToMinuteStart(dt).ToString("hh:mm tt", CultureInfo.InvariantCulture);
                case (TimeGranularity.Hours):
                    return TruncateToHourStart(dt).ToString("hh tt", CultureInfo.InvariantCulture);
                case (TimeGranularity.Days):
                default:
                    return TruncateToDayStart(dt).ToString("M/dd/yy");
            }
        }

        public static DateTime TruncateToDayStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        public static DateTime TruncateToHourStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }

        public static DateTime TruncateToMinuteStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }

        public static DateTime TruncateToSecondStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}
