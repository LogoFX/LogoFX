using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LogoFX.Server.DAL.DbContext
{
    public interface IDbContext
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        DbEntityEntry Entry(object o);
        void Dispose();
    }

    public interface IAuthenticationDbContext : IDisposable
    {
        System.Data.Entity.DbContext Context { get; }
    }

    public interface ITransactionFactory
    {
        DbContextTransaction CreateTransaction();
    }    
}