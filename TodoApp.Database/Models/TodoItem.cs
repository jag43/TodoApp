using System;
using System.Collections.Generic;

#nullable disable

namespace TodoApp.Database.Models
{
    public partial class TodoItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public long CreatedUnixTicks { get; set; }
        public string CreatedZoneId { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public long? DueUnixTicks { get; set; }
        public string DueZoneId { get; set; }
        public string Notes { get; set; }
    }
}
