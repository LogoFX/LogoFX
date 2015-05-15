using System.Data.Entity.Infrastructure;

namespace LogoFX.Server.DAL.DbContext
{
    public interface IDbContextConfiguration
    {
        DbContextConfiguration ContextConfiguration { get; }
    }    
}
