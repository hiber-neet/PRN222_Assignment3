using DataAccess.Models;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceContext _context;
        private IGenericRepository<Member> _members;
        private IGenericRepository<Order> _orders;
        private IGenericRepository<OrderDetail> _orderDetails;
        private IGenericRepository<Product> _products;
        private IGenericRepository<Category> _categories;
        
        public UnitOfWork(ECommerceContext context)
        {
            _context = context;
        }
        
        public IGenericRepository<Member> Members => 
            _members ??= new GenericRepository<Member>(_context);
            
        public IGenericRepository<Order> Orders => 
            _orders ??= new GenericRepository<Order>(_context);
            
        public IGenericRepository<OrderDetail> OrderDetails => 
            _orderDetails ??= new GenericRepository<OrderDetail>(_context);
            
        public IGenericRepository<Product> Products => 
            _products ??= new GenericRepository<Product>(_context);
            
        public IGenericRepository<Category> Categories => 
            _categories ??= new GenericRepository<Category>(_context);
        
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
} 