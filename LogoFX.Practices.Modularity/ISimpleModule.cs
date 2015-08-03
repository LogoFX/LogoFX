using System;
using LogoFX.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Practices.Modularity
{
    /// <summary>
    /// Module for <see cref="SimpleContainer"/>
    /// </summary>; 
    public interface ISimpleModule : ICompositionModule
    {
        /// <summary>
        /// Registers the module.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="lifetimeScopeAccess">The lifetime scope access.</param>
        void RegisterModule(SimpleContainer container, Func<object> lifetimeScopeAccess);
    }    
}
