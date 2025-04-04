
﻿using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
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
        private readonly MemberService _memberService;
        public OrderService(OrderRepository orderRepository, OrderDetailRepository orderDetailRepository,
            MemberService memberService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _memberService = memberService;
           
        }
        public async Task<List<Order>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrder();
        }
        public async Task CreateOrderAsync(Product product, int quantity, string email)
        {
            Order order = new();
            order.OrderDate = DateTime.Now;
            Member member = await _memberService.GetMemberByEmailAsync(email);
            order.MemberId = member.MemberId;
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
        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<Order>> GetOrdersByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _orderRepository.GetOrdersByPeriod(startDate, endDate);
        }

        public async Task<List<Order>> GetOrdersByMemberIdAsync(int memberId)
        {
            return await _orderRepository.GetOrdersByMemberId(memberId);
        }
    }

}
