using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LogoFX.Server.DAL.DbContext;
using LogoFX.Server.DAL.Entities;

namespace LogoFX.Server.DAL.Repository
{
    public sealed class Repository<TEntity> : IRepository<TEntity>, IRepositoryQueryProvider<TEntity> where TEntity : class
    {
        private readonly IDbSet<TEntity> _dbSet;

        public Repository(IDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public TEntity FindById(object id)
        {
            return _dbSet.Find(id);
        }

        public void InsertGraph(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
        }

        public void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            var objectState = entity as IObjectState;
            if (objectState != null) 
                objectState.State = ObjectState.Deleted;
            Delete(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Attach(entity);
        }

        public RepositoryQuery<TEntity> Query()
        {
            var repositoryGetFluentHelper =
                new RepositoryQuery<TEntity>(this);

            return repositoryGetFluentHelper;
        }

        public IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>>
                includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;
            
            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1)*pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }

	    
    }
}