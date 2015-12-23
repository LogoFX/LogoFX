using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Contracts;
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

        protected NavigationBootstrapper(bool useApplication = true)
            :base(useApplication)
        {
            
        }

        protected NavigationBootstrapper(TIocContainer iocContainer, bool useApplication=true, bool reuseCompositionInformation = false)
            :base(iocContainer, useApplication, reuseCompositionInformation)
        {
            
        }

        protected override void OnConfigure(TIocContainer container)
        {
            base.OnConfigure(container);
            container.RegisterInstance(NavigationService);
            OnRegisterRoot(NavigationService, container);
            OnPrepareNavigation(NavigationService, container);
        }        

        protected virtual void OnRegisterRoot(INavigationService navigationService, IIocContainer container)
        {
            RegisterRootViewModel(navigationService, container);
        }

        private static void RegisterRootViewModel(INavigationService navigationService, IIocContainer container)
        {
            navigationService.RegisterViewModel<TRootViewModel>(container).AsRoot();
        }

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