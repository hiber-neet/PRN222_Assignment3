using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Services
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderDetailRepository;

        public OrderDetailService(OrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId)
        {
            return await _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
        }
        public async Task DeleteOrderDetailByIdProduct(int id)
        {
            await _orderDetailRepository.DeleteOrderDetailByIdProduct(id);
        }
    }
}
