using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Asm03Solution.DataAccess.Repositories
{
    public class GenericRepository<T> where T : class
    {
		protected readonly ECommerceContext _context;

		public GenericRepository(ECommerceContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

		public async Task<int> UpdateAsync(T entity)
		{
			try
			{
				var primaryKey = GetPrimaryKey(entity); // Lấy giá trị primary key

				// Sử dụng FindAsync để tìm trực tiếp
				var existingEntity = await _context.Set<T>().FindAsync(primaryKey);

				if (existingEntity == null)
					throw new Exception($"Entity with key {primaryKey} not found");

				// Set giá trị mới vào entity cũ
				_context.Entry(existingEntity).CurrentValues.SetValues(entity);
				_context.Entry(existingEntity).State = EntityState.Modified;

				return await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("Update failed: " + ex.Message);
			}
		}

		private object GetPrimaryKey<T>(T entity)
        {
            var key = _context.Model.FindEntityType(typeof(T))
                .FindPrimaryKey()
                .Properties
                .Select(x => x.PropertyInfo.GetValue(entity))
                .FirstOrDefault();

            return key ?? throw new Exception("Primary key not found");
        }

        public async Task<T> GetByIdAsync(string code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

        }

        public T GetById(Guid code)
        {
            var entity = _context.Set<T>().Find(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;

        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            var entity = await _context.Set<T>().FindAsync(code);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

 
    }
}
