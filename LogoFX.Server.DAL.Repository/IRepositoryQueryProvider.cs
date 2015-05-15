using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LogoFX.Server.DAL.Repository
{
    public interface IRepositoryQueryProvider<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>>
                includeProperties = null,
            int? page = null,
            int? pageSize = null);
    }
}