using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace internship.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public bool Role { get; set; } = false; // Mặc định là False (không phải admin)

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
