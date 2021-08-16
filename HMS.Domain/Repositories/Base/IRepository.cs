using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMS.Domain.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true);
        //Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        //                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //                                List<Expression<Func<TEntity, object>>> includes = null,
        //                                bool disableTracking = true);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                        bool disableTracking = true);

        Task<IEnumerable<TEntity>> GetWithRawSqlAsync(string query, params object[] parameters);
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
