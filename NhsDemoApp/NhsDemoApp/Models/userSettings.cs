using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NhsDemoApp.Models
{
    public class UserSettings
    {
        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Organisation { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public int SecurityPin { get; set; }
        public string OnSiteID { get;set; }

    }
}
