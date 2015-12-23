using System;
using System.Collections.Generic;
using LogoFX.Client.Bootstrapping.Contracts;

namespace LogoFX.Client.Bootstrapping
{
    partial class BootstrapperContainerBase<TRootViewModel, TIocContainer>
    {
        private IBootstrapperAdapter _bootstrapperAdapter;

        protected override object GetInstance(Type service, string key)
        {
            return _bootstrapperAdapter.GetInstance(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _bootstrapperAdapter.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _bootstrapperAdapter.BuildUp(instance);
        }

        private void InitializeAdapter()
        {
            _bootstrapperAdapter = _iocContainer;
        }
    }
}
