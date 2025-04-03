using Asm03Solution.DataAccess.Models;
using Asm03Solution.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository
: GenericRepository<Product>
    {
        public ProductRepository(ECommerceContext context) : base(context)
        {
        }

		public List<Product> SearchProducts(string keyword, decimal? price)
		{
			var query = _context.Products.AsQueryable();

			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(p => p.ProductName.Contains(keyword));
			}

			if (price.HasValue)
			{
				query = query.Where(p => p.UnitPrice == price.Value);
			}

			return query.ToList();
		}
	}
}
