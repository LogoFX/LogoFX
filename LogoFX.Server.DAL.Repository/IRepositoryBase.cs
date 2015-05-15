using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LogoFX.Server.DAL.Repository
{
    public interface IRepositoryBase<TEntity>  where TEntity : class
    {
        /// <summary>
        /// Add an entity to the context.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(TEntity entity);

        /// <summary>
        /// Add multiple entities to the context.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddMultiple(List<TEntity> entities);

        /// <summary>
        /// Add an entity to the context or update the entity if it already exists.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddOrUpdate(TEntity entity);

        /// <summary>
        /// Add multiple entities to the context or update them if they already exsist.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddOrUpdateMultiple(List<TEntity> entities);

        /// <summary>
        /// Count all entities of a specific type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Int32 Count();

        bool Delete(TEntity entity);
        void Delete(object id);
        bool DeleteMultiple(List<TEntity> entities);

        void Detach(object entity);
        void Detach(List<object> entities);

        IQueryable Find(Expression<Func<TEntity, bool>> where);
        IQueryable Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);

        TEntity First(Expression<Func<TEntity, bool>> where);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);

        TEntity FindById(object id);

        RepositoryQuery<TEntity> Query();
        
        void SetIdentityCommand();       

        TEntity Single(Expression<Func<TEntity, bool>> where);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> include);

        bool Update(TEntity entity);
        bool UpdateProperty(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        bool UpdateMultiple(List<TEntity> entities);
    }

    public enum TransactionTypes
    {
        DbTransaction,
        TransactionScope
    }
}