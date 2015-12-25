using System;
using System.Collections.Generic;

namespace LogoFX.Client.Bootstrapping.Adapters.Contracts
{
    /// <summary>
    /// Represents an adapter of an IoC container to the Caliburn.Micro required IoC functionality
    /// </summary>
    public interface IBootstrapperAdapter
    {
        /// <summary>
        /// Resolves an instance of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <param name="key">Optional service key</param>
        /// <returns>Instance of service</returns>
        object GetInstance(Type serviceType, string key);

        /// <summary>
        /// Resolves all instances of required service by its type
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <returns>All instances of requested service</returns>
        IEnumerable<object> GetAllInstances(Type serviceType);

        /// <summary>
        /// Resolves instance's dependencies and injects them into the instance
        /// </summary>
        /// <param name="instance">Instance to get injected with dependencies</param>
        void BuildUp(object instance);
    }
}
