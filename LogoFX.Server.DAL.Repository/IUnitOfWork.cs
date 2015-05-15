using System;

namespace LogoFX.Server.DAL.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        IRepositoryBase<T> Repository<T>() where T : class;
    }
}