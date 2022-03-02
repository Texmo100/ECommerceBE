﻿using ECommerceBE.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBE.Models
{
    public class Supplier : IUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
