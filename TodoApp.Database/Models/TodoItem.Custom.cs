using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using TodoApp.Database.Helper;

namespace TodoApp.Database.Models
{
    public partial class TodoItem
    {
        public ZonedDateTime GetCreated(IDateTimeZoneProvider tzProvider)
        {
            return NodaTimeHelper.ConvertToZonedDateTime(CreatedUnixTicks, CreatedZoneId, tzProvider);
        }

        public void SetCreated(ZonedDateTime zonedDateTime)
        {

            CreatedUnixTicks = NodaTimeHelper.ConvertToUnixTicks(zonedDateTime);
            CreatedZoneId = zonedDateTime.Zone.Id;
        }

        public ZonedDateTime? GetDue(IDateTimeZoneProvider tzProvider)
        {
            return NodaTimeHelper.ConvertToZonedDateTime(DueUnixTicks, DueZoneId, tzProvider);
        }

        public void SetDue(ZonedDateTime? zonedDateTime)
        {
            DueUnixTicks = NodaTimeHelper.ConvertToUnixTicks(zonedDateTime);
            DueZoneId = zonedDateTime?.Zone.Id;
        }
    }
}