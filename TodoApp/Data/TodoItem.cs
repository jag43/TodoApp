using System;
using System.ComponentModel;
using TodoApp.Data.Sorting;
using NodaTime;

namespace TodoApp.Data
{
    public class TodoItem
    {
        public ZonedDateTime Created { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public ZonedDateTime? Due { get; set; }
        public string Notes { get; set; }
    }
}
 