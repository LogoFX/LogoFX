using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using LogoFX.Server.DAL.DbContext;

namespace LogoFX.Server.DAL.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>, IRepositoryQueryProvider<TEntity> where TEntity : class
    {
        private readonly IDbContext _dbContext;                
        
        public RepositoryBase(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(TEntity entity)
        {
            bool result = false;

            try
            {
                SetEntity().Add(entity);
                result = true;
            }
            catch (Exception)
            {
                var entry = SetEntry(entity);
                entry.State = EntityState.Unchanged;
                
                Detach(entity);                
            }

            return result;
        }

        public bool AddMultiple(List<TEntity> entities)
        {
            bool result = false;

            entities.ForEach(e => result = Add(e));

            return result;
        }

        public bool AddOrUpdate(TEntity entity)
        {
            bool result = false;

            try
            {
                var entry = SetEntry(entity);

                if (entry != null)
                {
                    entry.State = entry.State == EntityState.Detached ? EntityState.Added : EntityState.Modified;
                }
                else
                {
                    SetEntity().Attach(entity);
                }

                result = true;
            }
            catch (Exception)
            {
                var entry = SetEntry(entity);
                entry.State = EntityState.Unchanged;                
                Detach(entity);
            }

            return result;
        }

        public bool AddOrUpdateMultiple(List<TEntity> entities)
        {
            bool result = false;
            entities.ForEach(e => result = AddOrUpdate(e));
            return result;
        }        

        public Int32 Count()
        {
            return SetEntity()
                .Count();
        }

        public bool Delete(TEntity entity)
        {
            bool result = false;

            try
            {
                var entry = SetEntry(entity);

                if (entry != null)
                {
                    entry.State = EntityState.Deleted;
                }
                else
                {
                    SetEntity().Attach(entity);
                }

                SetEntity().Remove(entity);

                result = true;
            }
            catch (Exception)
            {
                var entry = SetEntry(entity);
                entry.State = EntityState.Unchanged;
                
                Detach(entity);
                
            }

            return result;
        }

        public void Delete(object id)
        {
            var entity = FindById(id);           
            Delete(entity);
        }

        public bool DeleteMultiple(List<TEntity> entities)
        {
            bool result = false;

            entities.ForEach(e => result = Delete(e));

            return result;
        }        

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
            var entry = SetEntry((TEntity)entity);

            if (entry.State != EntityState.Detached)
            {
                objectContext.Detach(entity);   
            }                
        }

        public void Detach(List<object> entities)
        {
            entities.ForEach(Detach);
        }

        public IQueryable Find(Expression<Func<TEntity, bool>> where)
        {
            IQueryable entities = SetEntities().Where(where);            

            return entities;
        }

        public IQueryable Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = SetEntities();

            if (includes != null)
            {
                entities = ApplyIncludesToQuery(entities, includes);
            }

            entities = entities.Where(where);

            return entities;
        }

        public TEntity First(Expression<Func<TEntity, bool>> where)
        {
            var entities = SetEntities();           

            var entity = entities
                    .First(where);

            return entity;
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = SetEntities();            

            if (where != null)
                entities = entities.Where(where);

            if (includes != null)
            {
                entities = ApplyIncludesToQuery(entities, includes);
            }

            var entity = entities.FirstOrDefault();

            return entity;
        }

        public IQueryable<TEntity> GetAll()
        {
            var entities = SetEntities();           

            return entities;
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = SetEntities();

            if (includes != null)
            {
                entities = ApplyIncludesToQuery(entities, includes);
            }

            return entities;
        }        

        public TEntity FindById(object id)
        {
            return SetEntity().Find(id);
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
            IQueryable<TEntity> query = SetEntity();

            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }

        public void SetIdentityCommand()
        {
            var container =
                ((IObjectContextAdapter)_dbContext).ObjectContext.MetadataWorkspace
                    .GetEntityContainer(
                        ((IObjectContextAdapter)_dbContext).ObjectContext.DefaultContainerName,
                        DataSpace.CSpace);

            var sets = container.BaseEntitySets.ToList();

            foreach (EntitySetBase set in sets)
            {
                string command = string.Format("SET IDENTITY_INSERT {0} {1}", set.Name, "ON");
                ((IObjectContextAdapter)_dbContext).ObjectContext.ExecuteStoreCommand(command);
            }
        }        
        
        public TEntity Single(Expression<Func<TEntity, bool>> where)
        {
            var entities = SetEntities();
            var entity = entities
                    .Single(where);            

            return entity;
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where)
        {
            var entities = SetEntities();
            var entity = entities
                    .SingleOrDefault(where);            

            return entity;
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> include)
        {
            var entities = SetEntities();
            var entity = entities
                    .Include(include)
                    .SingleOrDefault(where);            

            return entity;
        }

        public bool Update(TEntity entity)
        {
            bool result = false;

            try
            {
                var entry = SetEntry(entity);

                if (entry != null)
                {
                    entry.State = EntityState.Modified;
                }
                else
                {
                    SetEntity().Attach(entity);
                }

                result = true;
            }
            catch (Exception)
            {
                var entry = SetEntry(entity);
                entry.State = EntityState.Unchanged;
                
                Detach(entity);                
            }

            return result;
        }

        public bool UpdateProperty(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            bool result = false;

            try
            {
                var entry = SetEntry(entity);

                if (entry != null)
                {
                    SetEntity().Attach(entity);

                    foreach (var property in properties)
                    {
                        MemberExpression body = property.Body as MemberExpression;

                        if (body == null)
                        {
                            UnaryExpression ubody = (UnaryExpression)property.Body;
                            body = ubody.Operand as MemberExpression;
                        }

                        entry.Property(body.Member.Name).IsModified = true;
                    }
                }
                else
                {
                    SetEntity().Attach(entity);
                }

                result = true;
            }
            catch (Exception)
            {
                var entry = SetEntry(entity);

                foreach (var property in properties)
                {
                    MemberExpression body = property.Body as MemberExpression;

                    if (body == null)
                    {
                        UnaryExpression ubody = (UnaryExpression)property.Body;
                        body = ubody.Operand as MemberExpression;
                    }

                    entry.Property(body.Member.Name).IsModified = false;
                }
               
                Detach(entity);                
            }

            return result;
        }

        public bool UpdateMultiple(List<TEntity> entities)
        {
            bool result = false;

            entities.ForEach(e => result = Update(e));

            return result;
        }

        private IQueryable<TEntity> ApplyIncludesToQuery(IQueryable<TEntity> entities, Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null)
                entities = includes.Aggregate(entities, (current, include) => current.Include(include));

            return entities;
        }

        private IQueryable<TEntity> SetEntities()
        {
            var entities = _dbContext.Set<TEntity>();
            return entities;
        }

        private IDbSet<TEntity> SetEntity()
        {
            var entity = _dbContext.Set<TEntity>();

            return entity;
        }

        private DbEntityEntry SetEntry(TEntity entity)
        {
            DbEntityEntry entry = _dbContext.Entry(entity);

            return entry;
        }
    }
}