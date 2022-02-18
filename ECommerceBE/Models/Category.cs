using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBE.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool Enabled { get; set; } = true;
    }
}
