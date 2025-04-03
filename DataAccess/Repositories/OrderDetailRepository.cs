using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderDetailRepository
    {
        private readonly ECommerceContext _context;

        public OrderDetailRepository(ECommerceContext context)
        {
            _context = context;
        }
        public async Task CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
           
        }
    }
}
