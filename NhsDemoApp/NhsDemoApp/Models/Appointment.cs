using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NhsDemoApp.Models
{
    public class Appointment
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DueTime { get; set; }
        [Required]
        public string Contact { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ArrivalTime { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        public bool IsLate { get; set; }
        public bool OnSite { get; set; }
        public string User { get; set; }
        public string Organisation { get; set; }

    }
}
