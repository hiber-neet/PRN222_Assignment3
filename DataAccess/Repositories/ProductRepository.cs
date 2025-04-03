
﻿using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

﻿using DataAccess.Models;
using DataAccess.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository

    {
        private readonly ECommerceContext _context;

        public ProductRepository(ECommerceContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProductsAsync()
        {
            return _context.Products.Include(p => p.Category).ToList<Product>();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public Product GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);
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

