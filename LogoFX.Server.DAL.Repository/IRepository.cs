﻿namespace LogoFX.Server.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity FindById(object id);
        void InsertGraph(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Insert(TEntity entity);
        RepositoryQuery<TEntity> Query();
    }
}
