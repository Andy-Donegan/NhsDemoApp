using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NhsDemoApp.Models
{
    public class User
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
    }
}
