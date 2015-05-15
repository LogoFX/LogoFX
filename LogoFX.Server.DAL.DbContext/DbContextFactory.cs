using System;

namespace LogoFX.Server.DAL.DbContext
{
    public interface IDbContextFactory
    {
        TContext CreateDbContext<TContext>() where TContext : System.Data.Entity.DbContext;
    }

    public class DbContextFactory : IDbContextFactory
    {
        public TContext CreateDbContext<TContext>() where TContext : System.Data.Entity.DbContext
        {
            return (TContext)Activator.CreateInstance(typeof(TContext));
        }
    }
}
