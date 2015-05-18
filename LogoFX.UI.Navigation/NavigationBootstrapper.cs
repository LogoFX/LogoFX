using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using LogoFX.Core;
using LogoFX.UI.Bootstraping.Contracts;
using LogoFX.UI.Bootstrapping;
using Solid.Practices.IoC;

namespace LogoFX.UI.Navigation
{
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

        protected NavigationBootstrapper(TIocContainer iocContainer, bool useApplication=true)
            :base(iocContainer, useApplication)
        {
            
        }

        protected override void OnConfigure(TIocContainer container)
        {
            base.OnConfigure(container);
            container.RegisterInstance(NavigationService);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);            
            OnRegisterRoot(NavigationService);
            OnPrepareNavigation(NavigationService);
        }

        protected virtual void OnRegisterRoot(INavigationService navigationService)
        {
            navigationService.RegisterViewModel<TRootViewModel>().AsRoot();
        }

        protected virtual void OnPrepareNavigation(INavigationService navigationService)
        {
            //register navigation view-models
            AssemblySource.Instance.ToArray()
                .SelectMany(ass => ass.GetTypes())
                .Where(type => type != typeof(TRootViewModel) && type.IsClass)
                .Select(type => new
                {
                    Type = type,
                    Attr = type.GetCustomAttribute<NavigationViewModelAttribute>()
                })
                .Where(x => x.Attr != null)
                .ForEach(x => _navigationService.RegisterAttr(x.Type, x.Attr));
        }
    }
}