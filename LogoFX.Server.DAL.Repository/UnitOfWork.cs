using System;
using System.Collections;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using LogoFX.Server.DAL.DbContext;

namespace LogoFX.Server.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _dbContext;
       
        private readonly ITransactionFactory _transactionFactory;

        private bool _disposed;
        private Hashtable _repositories;

        private const int CommandTimeout = 300;


        private DbConnection _connection;

        public UnitOfWork(IDbContext dbContext,  ITransactionFactory transactionFactory)
        {
            _dbContext = dbContext;
  
            _transactionFactory = transactionFactory;
          
            InitializeUnitOfWork();

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _dbContext.Dispose();                    
                }                    
            }
                
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            using (var contextTransaction = _transactionFactory.CreateTransaction())
            {
                try
                {
                    _dbContext.SaveChanges();
                    contextTransaction.Commit();
                }
                catch (Exception)
                {                    
                    contextTransaction.Rollback();
                }                
            }            
        }

        public IRepositoryBase<T> Repository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                            .MakeGenericType(typeof(T)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepositoryBase<T>)_repositories[type];
        }



        private void InitializeUnitOfWork()
        {
            ((IObjectContextAdapter)_dbContext).ObjectContext.CommandTimeout = CommandTimeout;
            _connection = ((IObjectContextAdapter)_dbContext).ObjectContext.Connection;
            _connection.Open();
        }

    }
}
