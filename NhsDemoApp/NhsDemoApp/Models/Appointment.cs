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
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DepartureTime { get; set; }
        [Required]
        bool _IsCompleted;
        public bool IsCompleted
        {
            get
            {
                return _IsCompleted;
            }
            set
            {
                _IsCompleted = value;
            }
        }
        public bool IsLate { get; set; }
        public bool OnSite { get; set; }
        public string User { get; set; }
        public string Organisation { get; set; }
        public bool TimesRequired { get; set; }

    }
}
