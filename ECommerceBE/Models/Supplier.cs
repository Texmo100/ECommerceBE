﻿using ECommerceBE.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBE.Models
{
    public class Supplier : IUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
