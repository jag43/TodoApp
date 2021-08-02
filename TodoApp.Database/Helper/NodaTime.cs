using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace TodoApp.Database.Helper
{
    public class NodaTimeHelper
    {

        public static ZonedDateTime? ConvertToZonedDateTime(long? ticks, string zoneId, IDateTimeZoneProvider tzProvider)
        {
            if (ticks.HasValue && !string.IsNullOrWhiteSpace(zoneId))
            {
                return ConvertToZonedDateTime(ticks.Value, zoneId, tzProvider);
            }

            return null;
        }

        public static ZonedDateTime ConvertToZonedDateTime(long ticks, string zoneId, IDateTimeZoneProvider tzProvider)
        {
            return Instant.FromUnixTimeTicks(ticks).InZone(tzProvider[zoneId]);
        }

        public static long? ConvertToUnixTicks(ZonedDateTime? zonedDateTime)
        {
            if (zonedDateTime.HasValue)
            {
                return ConvertToUnixTicks(zonedDateTime.Value);
            }
            return null;
        }

        public static long ConvertToUnixTicks(ZonedDateTime zonedDateTime)
        {
            return zonedDateTime.ToInstant().ToUnixTimeTicks();
        }
    }
}
