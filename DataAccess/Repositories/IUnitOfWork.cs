using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Gets the repository for entity of type T
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>Generic repository for that entity type</returns>
        IGenericRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Commits all changes to the database
        /// </summary>
        /// <returns>The number of affected rows</returns>
        int Commit();

        /// <summary>
        /// Commits all changes to the database asynchronously
        /// </summary>
        /// <returns>The number of affected rows</returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Begins a new database transaction
        /// </summary>
        /// <returns>The transaction object</returns>
        Task<IDbContextTransaction> BeginTransactionAsync();

        /// <summary>
        /// Saves changes to the database without completing a transaction
        /// </summary>
        /// <returns>The number of affected rows</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Rolls back the current transaction
        /// </summary>
        Task RollbackTransactionAsync();
    }
}