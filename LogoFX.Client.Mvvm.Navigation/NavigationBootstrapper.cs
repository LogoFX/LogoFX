using System.Linq;
using System.Reflection;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Core;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation bootstrapper which registers the common navigation facilities into the IoC Container; 
    /// this includes the navigation view models
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the IoC container adapter.</typeparam>
    public class NavigationBootstrapper<TRootViewModel, TIocContainerAdapter> : 
        BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> 
        where TRootViewModel : class where TIocContainerAdapter : class, IIocContainer, IBootstrapperAdapter, new()
    {
        private NavigationService _navigationService = new NavigationService();        
        private INavigationService NavigationService
        {
            get { return _navigationService ?? (_navigationService = new NavigationService()); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBootstrapper{TRootViewModel, TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>        
        protected NavigationBootstrapper(TIocContainerAdapter iocContainerAdapter)
            : this(iocContainerAdapter, new BootstrapperCreationOptions())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBootstrapper{TRootViewModel, TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="creationOptions">The creation options.</param>
        protected NavigationBootstrapper(TIocContainerAdapter iocContainerAdapter, BootstrapperCreationOptions creationOptions)
            :base(iocContainerAdapter, creationOptions)
        {
            
        }

        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="iocContainerAdapter">IoC container adapter.</param>
        protected override void OnConfigure(TIocContainerAdapter iocContainerAdapter)
        {
            base.OnConfigure(iocContainerAdapter);
            iocContainerAdapter.RegisterInstance(NavigationService);
            OnRegisterRoot(NavigationService, iocContainerAdapter);
            OnPrepareNavigation(NavigationService, iocContainerAdapter);
        }        

        /// <summary>
        /// Override this method to inject custom logic during root view model registration.
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="container"></param>
        protected virtual void OnRegisterRoot(INavigationService navigationService, IIocContainer container)
        {
            RegisterRootViewModel(navigationService, container);
        }

        private static void RegisterRootViewModel(INavigationService navigationService, IIocContainer container)
        {
            navigationService.RegisterViewModel<TRootViewModel>(container).AsRoot();
        }

        /// <summary>
        /// Override this method to inject custom logic during navigation view models registration.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="container">The IoC container.</param>
        protected virtual void OnPrepareNavigation(INavigationService navigationService, IIocContainer container)
        {
            RegisterNavigationViewModels(container);
        }

        private void RegisterNavigationViewModels(IIocContainer container)
        {
            Assemblies.ToArray()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type != typeof (TRootViewModel) && type.IsClass)
                .Select(type => new
                {
                    Type = type,
                    Attr = type.GetCustomAttribute<NavigationViewModelAttribute>()
                })
                .Where(x => x.Attr != null)
                .ForEach(x => _navigationService.RegisterAttribute(x.Type, x.Attr, container));
        }
    }
}