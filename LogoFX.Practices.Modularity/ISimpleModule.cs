using System;
using LogoFX.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Practices.Modularity
{
    /// <summary>
    /// Composition module for <see cref="SimpleContainer"/>
    /// </summary>; 
    public interface ISimpleModule : ICompositionModule
    {
        /// <summary>
        /// Registers the composition module into the <see cref="SimpleContainer"/>
        /// </summary>
        /// <param name="container">The simple container.</param>
        /// <param name="lifetimeScopeAccess">The lifetime scope access lambda expression</param>
        void RegisterModule(SimpleContainer container, Func<object> lifetimeScopeAccess);
    }    
}
