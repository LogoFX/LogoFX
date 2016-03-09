using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;
using LogoFX.Core;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Contains cross-platform bootstrapper utilities.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    public static class BootstrapperHelper<TRootViewModel, TIocContainerAdapter> 
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer
    {
        /// <summary>
        /// Initializes the framework dispatcher.
        /// </summary>
        public static void InitializeDispatcher()
        {
            Dispatch.Current.InitializeDispatch();
        }

        /// <summary>
        /// Registers the IoC container and root view model.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        public static void RegisterCore(TIocContainerAdapter iocContainerAdapter)                                    
        {
            iocContainerAdapter.RegisterSingleton<TRootViewModel, TRootViewModel>();
            iocContainerAdapter.RegisterInstance(iocContainerAdapter);
            iocContainerAdapter.RegisterInstance<IIocContainer>(iocContainerAdapter);            
        }

        /// <summary>
        /// Registers the views and view models.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="assemblies">The assemblies.</param>
        public static void RegisterViewsAndViewModels(IIocContainerRegistrator iocContainer, IEnumerable<Assembly> assemblies)
        {
            assemblies
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => type != typeof (TRootViewModel) && type.Name.EndsWith("ViewModel"))
                .Where(
                    type =>
                        !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace != null &&
                        type.Namespace.EndsWith("ViewModels"))
                .Where(type => type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(INotifyPropertyChanged)))
                .ForEach(a => iocContainer.RegisterTransient(a, a));
        }

        /// <summary>
        /// Registers the composition modules.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="modules">The modules.</param>
        public static void RegisterCompositionModules(
            TIocContainerAdapter iocContainerAdapter, 
            IEnumerable<ICompositionModule> modules)
        {
            new ModuleRegistrator(modules).RegisterModules(iocContainerAdapter);
        }        
    }
}
