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
    /// <typeparam name="TIocContainer">The type of the IoC container.</typeparam>
    public class NavigationBootstrapper<TRootViewModel, TIocContainer> : 
        BootstrapperContainerBase<TRootViewModel, TIocContainer> 
        where TRootViewModel : class where TIocContainer : class, IIocContainer, IBootstrapperAdapter, new()
    {
        private NavigationService _navigationService = new NavigationService();        
        private INavigationService NavigationService
        {
            get { return _navigationService ?? (_navigationService = new NavigationService()); }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NavigationBootstrapper{TRootViewModel,TIocContainer}"/> class.
        /// </summary>
        /// <param name="useApplication"></param>
        protected NavigationBootstrapper(bool useApplication = true)
            :base(useApplication)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBootstrapper{TRootViewModel, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="useApplication">if set to <c>true</c> [use application].</param>
        /// <param name="reuseCompositionInformation">if set to <c>true</c> [reuse composition information].</param>
        protected NavigationBootstrapper(TIocContainer iocContainer, bool useApplication=true, bool reuseCompositionInformation = false)
            :base(iocContainer, useApplication, reuseCompositionInformation)
        {
            
        }

        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="container">IoC container</param>
        protected override void OnConfigure(TIocContainer container)
        {
            base.OnConfigure(container);
            container.RegisterInstance(NavigationService);
            OnRegisterRoot(NavigationService, container);
            OnPrepareNavigation(NavigationService, container);
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