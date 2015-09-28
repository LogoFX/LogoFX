using System;
using System.Collections.Generic;

namespace LogoFX.Client.Bootstrapping.Contracts
{
    /// <summary>
    /// Represents an adapter of an IoC container to the Caliburn.Micro required IoC functionality
    /// </summary>
    public interface IBootstrapperAdapter
    {
        /// <summary>
        /// Resolves an explicitly-typed instance of required service by its type
        /// </summary>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <param name="serviceType">Explicit type of service</param>
        /// <returns>Instance of service</returns>
        TService GetInstance<TService>(Type serviceType) where TService : class;

        /// <summary>
        /// Resolves an explicitly-typed instance of required service by its type
        /// </summary>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <returns>Instance of service</returns>
        TService GetInstance<TService>() where TService : class;

        /// <summary>
        /// Resolves an instance of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>Instance of service</returns>
        object GetInstance(Type serviceType);

        /// <summary>
        /// Resolves all instances of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>Instance of service</returns>
        IEnumerable<object> GetAllInstances(Type serviceType);

        /// <summary>
        /// Resolves instance's dependencies and injects them into the instance
        /// </summary>
        /// <param name="instance">Instance to get injected with dependencies</param>
        void BuildUp(object instance);
    }
}
