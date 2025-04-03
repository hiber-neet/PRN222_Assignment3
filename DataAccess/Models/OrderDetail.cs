using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderId { get; set; }

        [Key]
        public int ProductId { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Discount { get; set; }

        // Navigation properties
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
