using System;
using System.Collections.Generic;
using System.Text;

namespace NhsDemoApp.Models
{
    public class Appointment
    {
        public string Id { get; set; }
        public DateTime DueTime { get; set; }
        public string Contact { get; set; }
        public DateTime ArrivalTime { get; set; }
        public bool IsCompleted { get; set; }
        public string User { get; set; }
        public string Organisation { get; set; }

    }
}
