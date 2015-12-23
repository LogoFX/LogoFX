using System;
using System.Collections.Generic;
using System.Linq;
using LogoFX.Practices.Modularity;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping.SimpleContainer
{
    class SimpleModuleMiddleware : IMiddleware
    {
        private readonly IEnumerable<ICompositionModule> _modules;
        private readonly Func<object> _lifetimeScopeAccess;

        public SimpleModuleMiddleware(IEnumerable<ICompositionModule> modules, Func<object> lifetimeScopeAccess)
        {
            _modules = modules;
            _lifetimeScopeAccess = lifetimeScopeAccess;
        }

        public TIocContainer Apply<TIocContainer>(TIocContainer iocContainer) where TIocContainer : class, IIocContainer
        {
            var simpleModules = _modules.OfType<ISimpleModule>();
            //TODO: this smells a lot
            var innerContainer = iocContainer.Resolve<Practices.IoC.SimpleContainer>();
            foreach (var simpleModule in simpleModules)
            {
                simpleModule.RegisterModule(innerContainer, _lifetimeScopeAccess);
            }
            return iocContainer;
        }
    }
}