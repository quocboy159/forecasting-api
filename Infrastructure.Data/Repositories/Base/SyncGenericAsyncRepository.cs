using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace ForecastingSystem.Infrastructure.Data.Repositories.Base
{
    public class SyncGenericAsyncRepository<T> : IAsyncRepository<T> where T : class
    {
        protected SyncForecastingSystemDbContext DbContext { get; }
        protected DbSet<T> DbSet => DbContext.Set<T>();

        public SyncGenericAsyncRepository(SyncForecastingSystemDbContext context)
        {
            DbContext = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            return await Task.FromResult(entity);
        }

        public async Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities)
        {
            DbSet.UpdateRange(entities);
            return await Task.FromResult(entities);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            return await Task.FromResult(entity);
        }

        public async Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            return await Task.FromResult(entities);
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await DbContext.FindAsync<T>(id) != null;
        }

        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.AsNoTracking().CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).AnyAsync();
        }
    }
}
