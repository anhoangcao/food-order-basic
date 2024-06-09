using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace internship.Models
{
    public class OrderItemModel
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ItemID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Navigation properties for Order and MenuItem (foreign key relationships)
        public OrderModel Order { get; set; } = new OrderModel();
        public MenuItemModel MenuItem { get; set; } = new MenuItemModel();
    }
}
