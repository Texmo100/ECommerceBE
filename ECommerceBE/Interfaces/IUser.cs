using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBE.Interfaces
{
    public interface IUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(75)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 8, ErrorMessage = "The password must have between 8 and 25 characters")]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        [Phone]
        [StringLength(maximumLength: 14, MinimumLength = 8, ErrorMessage = "The Phone Number must have between 8 ans 14 characters")]
        public string PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
