using DataAccess.Models;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Member> Members { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderDetail> OrderDetails { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Category> Categories { get; }
        
        Task<int> CompleteAsync();
    }
} 