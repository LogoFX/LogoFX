using System;
using System.Threading;

namespace LogoFX.Practices.IoC.Extensions.SimpleInjector
{
    /// <summary>
    /// Used for resolving dependencies with paramters from <see cref="SimpleInjector"/> IoC container
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class ObjectResolver : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectResolver"/> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public ObjectResolver(object parameter)
        {
            var localDataStoreSlot = Thread.AllocateNamedDataSlot(Consts.ModelParameterName);
            Thread.SetData(localDataStoreSlot, parameter);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Thread.FreeNamedDataSlot(Consts.ModelParameterName);
        }
    }
}