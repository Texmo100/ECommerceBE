using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceBE.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        public double Weight { get; set; }
        public string Description { get; set; }
        //public string FolderImagesUrl { get; set; }

        [Required]
        public int Stock { get; set; }

        public bool Enabled { get; set; } = true;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        [Required]
        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
    }
}
