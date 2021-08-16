using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HMS.Domain;
using HMS.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HMS.Data.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly HMSContext _context;
        
        public Repository(HMSContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeString = null,
            bool disableTracking = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(includeString))
            {
                query.Include(includeString);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        //public async Task<IEnumerable<TEntity>> GetAsync(
        //    Expression<Func<TEntity, bool>> predicate = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //    List<Expression<Func<TEntity, object>>> includes = null,
        //    bool disableTracking = false)
        //{
        //    IQueryable<TEntity> query = _context.Set<TEntity>();
        //    if (disableTracking)
        //    {
        //        query = query.AsNoTracking();
        //    }
        //    if (includes != null)
        //    {
        //        query = includes.Aggregate(query, (current, include) => current.Include(include));
        //    }
        //    if (predicate != null)
        //    {
        //        query = query.Where(predicate);
        //    }
        //    if (orderBy != null)
        //    {
        //        return await orderBy(query).ToListAsync();
        //    }
        //    return await query.ToListAsync();
        //}


        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = false)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> GetWithRawSqlAsync(string query, params object[] parameters)
        {
            return await _context.Set<TEntity>().FromSqlRaw(query, parameters).ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
