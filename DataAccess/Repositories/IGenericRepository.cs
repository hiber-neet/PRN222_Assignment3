using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // Get all entities
        Task<IEnumerable<T>> GetAllAsync();
        
        // Get entities with filter
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        
        // Get entity by id
        Task<T> GetByIdAsync(object id);
        
        // Add new entity
        Task AddAsync(T entity);
        
        // Add multiple entities
        Task AddRangeAsync(IEnumerable<T> entities);
        
        // Update an entity
        void Update(T entity);
        
        // Remove an entity
        void Remove(T entity);
        
        // Remove multiple entities
        void RemoveRange(IEnumerable<T> entities);
    }
} 