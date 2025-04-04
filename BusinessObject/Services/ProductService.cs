
﻿using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private OrderDetailService _orderDetailService;
        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProductsAsync()
        {
            return  _productRepository.GetAllProductsAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _productRepository.GetProductsByCategoryAsync(categoryId);
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }
    

		public List<Product> SearchProducts(string keyword, decimal? price)
		{
			return _productRepository.SearchProducts(keyword, price);
		}
        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
        }
        public async Task DeleteProductAsync(int id)
        {
            
            await _productRepository.DeleteProductAsync(id);
        }
        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateProductAsync(product);
        }
    }

}
