﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceBE.Interfaces
{
    public interface IUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
