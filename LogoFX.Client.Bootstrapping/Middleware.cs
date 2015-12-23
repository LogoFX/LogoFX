using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using LogoFX.Core;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    class CommonMiddleware<TRootViewModel> : IMiddleware where TRootViewModel : class
    {
        public TIocContainer Apply<TIocContainer>(TIocContainer iocContainer) where TIocContainer : class, IIocContainer
        {
            iocContainer.RegisterSingleton<IWindowManager, WindowManager>();
            iocContainer.RegisterSingleton<TRootViewModel, TRootViewModel>();
            iocContainer.RegisterInstance<IIocContainer>(iocContainer);
            iocContainer.RegisterInstance(iocContainer);
            return iocContainer;
        }
    }

    class ViewAndViewModelMiddleware<TRootViewModel> : IMiddleware
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public ViewAndViewModelMiddleware(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        public TIocContainer Apply<TIocContainer>(TIocContainer iocContainer) where TIocContainer : class, IIocContainer
        {
            _assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type != typeof(TRootViewModel) && type.Name.EndsWith("ViewModel"))
                .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace != null && type.Namespace.EndsWith("ViewModels"))
                .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name, false) != null)
                .ForEach(a => iocContainer.RegisterTransient(a, a));
            return iocContainer;
        }
    }

    class ModuleMiddleware : IMiddleware
    {
        private readonly IEnumerable<ICompositionModule> _modules;

        public ModuleMiddleware(IEnumerable<ICompositionModule> modules)
        {
            _modules = modules;
        }

        public TIocContainer Apply<TIocContainer>(TIocContainer iocContainer) where TIocContainer : class, IIocContainer
        {
            var moduleRegistrator = new ModuleRegistrator(_modules);
            moduleRegistrator.RegisterModules(iocContainer);
            return iocContainer;            
        }
    }
}
