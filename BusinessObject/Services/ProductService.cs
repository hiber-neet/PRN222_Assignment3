using Asm03Solution.DataAccess.Models;
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

		public ProductService(ProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public List<Product> SearchProducts(string keyword, decimal? price)
		{
			return _productRepository.SearchProducts(keyword, price);
		}
	}
}
