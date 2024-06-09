using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace internship.Models
{
    public class MenuItemModel
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        [MaxLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public bool Available { get; set; } = true;

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}