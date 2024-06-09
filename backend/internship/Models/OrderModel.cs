using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace internship.Models
{
    public class OrderModel
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; } = "Pending"; // Mặc định là Pending

        [MaxLength(255)]
        public string DeliveryAddress { get; set; } = string.Empty;

        [MaxLength(255)]
        public string PaymentDetails { get; set; } = string.Empty;

        // Navigation property for User (foreign key relationship)
        public UserModel User { get; set; } = new UserModel();
    }
}
