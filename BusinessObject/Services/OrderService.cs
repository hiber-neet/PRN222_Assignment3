using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly OrderDetailRepository _orderDetailRepository;
        public OrderService(OrderRepository orderRepository, OrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task CreateOrderAsync(Product product, int quantity, string email)
        {
            Order order = new();
            order.OrderDate = DateTime.Now;
            order.MemberId = 1;
            int orderId = await _orderRepository.CreateOrderAsync(order);
            OrderDetail orderDetail = new();
            orderDetail.OrderId = orderId;
            orderDetail.ProductId = product.ProductId;
            orderDetail.Quantity = quantity;
            orderDetail.UnitPrice = product.UnitPrice * quantity;
            orderDetail.Discount = 2.0f;
            await _orderDetailRepository.CreateOrderDetailAsync(orderDetail);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }
    }   
}
