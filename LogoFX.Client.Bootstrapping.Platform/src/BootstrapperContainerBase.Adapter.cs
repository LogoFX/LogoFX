using System;
using System.Collections.Generic;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;

namespace LogoFX.Client.Bootstrapping
{
    partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>
    {
        private IBootstrapperAdapter _bootstrapperAdapter;

        /// <summary>
        /// Gets the service by its type and optional <see cref="string"/> key.
        /// Not intended to be used explicitly from code.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected override object GetInstance(Type service, string key)
        {            
            return _bootstrapperAdapter.GetInstance(service, key);
        }

        /// <summary>
        /// Gets all instances of service by its type.
        /// Not intended to be used explicitly from code.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _bootstrapperAdapter.GetAllInstances(service);
        }

        /// <summary>
        /// Injects all missing properties from the IoC containerAdapter into the provided object.
        /// Not intended to be used explicitly from code.
        /// </summary>
        /// <param name="instance">The instance.</param>
        protected override void BuildUp(object instance)
        {
            _bootstrapperAdapter.BuildUp(instance);
        }

        private void InitializeAdapter()
        {
            _bootstrapperAdapter = _iocContainerAdapter;
        }
    }
}
