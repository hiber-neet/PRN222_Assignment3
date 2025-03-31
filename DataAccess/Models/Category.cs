using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(40)]
        public string CategoryName { get; set; }


        public string Description { get; set; }

        // Navigation property
        public ICollection<Product> Products { get; set; }
    }
}
