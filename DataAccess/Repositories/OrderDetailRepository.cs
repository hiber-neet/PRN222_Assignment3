
﻿using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
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
        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

    }
}
