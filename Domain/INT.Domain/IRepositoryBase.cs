using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using INT.Domain.DataTable;

namespace INT.Domain
{
    public enum EntityTracking
    {
        Detached = 0,
        Unchanged = 1,
        Deleted = 2,
        Modified = 3,
        Added = 4
    }

    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int countRecords = default(int), bool asNoTracking = false);
        Task<ICollection<TEntity>> GetAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int countRecords = default(int), bool asNoTracking = false);
        Task<TEntity> GetByIdAsync(object id);
        ValueTask<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken);
        ICollection<TEntity> GetAll();
        Task<ICollection<TEntity>> GetAllAsync();
        Task<ICollection<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task DeleteAsync(object id);
        Task DeleteAsync(object id, CancellationToken cancellationToken);
        Task DeleteAsync(TEntity entityToDelete);
        Task DeleteAsync(TEntity entityToDelete, CancellationToken cancellationToken);
        Task DeleteRangeAsync(ICollection<TEntity> collection);
        Task DeleteRangeAsync(ICollection<TEntity> collection, CancellationToken cancellationToken);
        Task AddAsync(TEntity entityToAdd);
        Task AddRangeAsync(ICollection<TEntity> collection);
        Task AddRangeAsync(ICollection<TEntity> collection, CancellationToken cancellationToken);
        Task AttachAsync(TEntity entityToAttach);
        Task AttachAsync(TEntity entityToAttach, CancellationToken cancellationToken);
        Task AttachRangeAsync(ICollection<TEntity> collection);
        Task AttachRangeAsync(ICollection<TEntity> collection, CancellationToken cancellationToken);
        Task UpdateAsync(TEntity entityToUpdate);
        Task UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken);
        Task UpdateRangeAsync(ICollection<TEntity> collection);
        Task UpdateRangeAsync(ICollection<TEntity> collection, CancellationToken cancellationToken);
        Expression<Func<TEntityCustom, bool>> CreateFilter<TEntityCustom>(List<Filter> filters);
        List<TEntityCustom> CreateSort<TEntityCustom, TPropertyType>(IEnumerable<TEntityCustom> collection, Sort sort);
        Task NoTrackingObject<TPropertyType>(TEntity entity, Expression<Func<TEntity, TPropertyType>> propertyExpression) where TPropertyType : class;
        Task NoTrackingCollection<TPropertyType>(TEntity entity, Expression<Func<TEntity, IEnumerable<TPropertyType>>> propertyExpression) where TPropertyType : class;
        Task NoTrackingProperty<TPropertyType>(TEntity entity, Expression<Func<TEntity, TPropertyType>> propertyExpression);
        Task ChangeStateTracking(TEntity entity, EntityTracking entityTracking);
        Task<ICollection<TEntity>> Include(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);
        Task<ICollection<TEntity>> Include(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int countRecords = default(int), bool asNoTracking = false, params Expression<Func<TEntity, object>>[] includes);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
    }
}
