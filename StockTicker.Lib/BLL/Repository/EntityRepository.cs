using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PagedList;
using StockTicker.Lib.Common.Enums;
using StockTicker.Lib.DAL.EF;

namespace StockTicker.Lib.BLL.Repository
{
    /// <summary>
    ///     IEntityRepository implementation for DbContext instance.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TId">Type of entity Id</typeparam>
    public abstract class EntityRepository<TEntity, TId> : IEntityRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : IComparable
    {
        private readonly IDbContext _dbContext;

        protected EntityRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().Where(w => w.IsDeleted != true);
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = GetAll();

            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            var queryable = GetAll().Where(predicate);

            return queryable;
        }

        public IPagedList<TEntity> Paginate(int pageIndex, int pageSize)
        {
            var paginatedList = Paginate(pageIndex, pageSize, x => x.Id);

            return paginatedList;
        }

        public IPagedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> keySelector)
        {
            return Paginate(pageIndex, pageSize, keySelector, null);
        }

        public IPagedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var paginatedList = Paginate(pageIndex, pageSize, keySelector, predicate, OrderByType.Ascending,includeProperties);

            return paginatedList;
        }

        public IPagedList<TEntity> PaginateDescending<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> keySelector)
        {
            return PaginateDescending(pageIndex, pageSize, keySelector, null);
        }

        public IPagedList<TEntity> PaginateDescending<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var paginatedList = Paginate(pageIndex, pageSize, keySelector, predicate, OrderByType.Descending,includeProperties);

            return paginatedList;
        }

        public TEntity GetSingle(TId id)
        {
            var entities = GetAll();
            var entity = Filter(entities, x => x.Id, id).FirstOrDefault();

            return entity;
        }

        public TEntity GetSingleIncluding(TId id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = GetAllIncluding(includeProperties);
            var entity = Filter(entities, x => x.Id, id).FirstOrDefault();

            return entity;
        }

        public void Add(TEntity entity)
        {
            _dbContext.SetAsAdded(entity);
        }

        public void AddGraph(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Edit(TEntity entity)
        {
            _dbContext.SetAsModified(entity);
        }

        public void Delete(TEntity entity, bool directly = false)
        {
            if (directly)
            {
                _dbContext.SetAsDeleted(entity);
            }
            else
            {
                entity.IsDeleted = true;

                _dbContext.SetAsModified(entity);
            }
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public virtual void EditGraph(TEntity entity)
        {
            //TODO:use GraphDiff
        }

        public IQueryable<TEntity> GetIncluding(TId id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = GetAll();

            queryable = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));

            var entity = Filter(queryable, x => x.Id, id);

            return entity;
        }

        public virtual void AddOrEdit(params TEntity[] entities)
        {
        }

        private IPagedList<TEntity> Paginate<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, TKey>> keySelector, Expression<Func<TEntity, bool>> predicate,
            OrderByType orderByType, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable =
                orderByType == OrderByType.Ascending
                    ? GetAllIncluding(includeProperties).OrderBy(keySelector)
                    : GetAllIncluding(includeProperties).OrderByDescending(keySelector);

            queryable = predicate != null ? queryable.Where(predicate) : queryable;

            var paginatedList = queryable.ToPagedList(pageIndex, pageSize);

            return paginatedList;
        }

        private IQueryable<TEntity> Filter<TProperty>(IQueryable<TEntity> dbSet,
            Expression<Func<TEntity, TProperty>> property, TProperty value)
            where TProperty : IComparable
        {
            var memberExpression = property.Body as MemberExpression;

            if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
                throw new ArgumentException("Property expected", nameof(property));

            var left = property.Body;
            var right = Expression.Constant(value, typeof(TProperty));
            var searchExpression = Expression.Equal(left, right);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(searchExpression, property.Parameters.Single());

            return dbSet.Where(lambda);
        }
    }

    /// <summary>
    ///     IEntityRepository implementation for DbContext instance where the TId type is Int32.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    public class EntityRepository<TEntity> : EntityRepository<TEntity, long>, IEntityRepository<TEntity>
        where TEntity : class, IEntity<long>
    {
        public EntityRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}