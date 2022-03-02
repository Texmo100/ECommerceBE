using ECommerceBE.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBE.Models
{
    public class Customer : IUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        
        public string Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; } = true;//TODO change this field when email validation is available
        public bool Enabled { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
