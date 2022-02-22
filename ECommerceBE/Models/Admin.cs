﻿using ECommerceBE.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBE.Models
{
    public class Admin : IUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
