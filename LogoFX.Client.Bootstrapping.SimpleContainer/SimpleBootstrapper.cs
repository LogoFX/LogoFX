using System.Linq;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping.SimpleContainer
{
    public class SimpleBootstrapper<TRootViewModel> : BootstrapperContainerBase<TRootViewModel,ExtendedSimpleIocContainer> where TRootViewModel : class
    {
        public SimpleBootstrapper(ExtendedSimpleIocContainer container, bool useApplication = true, bool reuseCompositionInformation = false)
            :base(container, useApplication, reuseCompositionInformation)
        {
            
        }

        public SimpleBootstrapper(bool useApplication = true, bool reuseCompositionInformation = false)
            :base(useApplication, reuseCompositionInformation)
        {
            
        }

        protected override void OnConfigure(ExtendedSimpleIocContainer container)
        {
            base.OnConfigure(container);            
            var simpleModules = Modules.OfType<ISimpleModule>();
            //TODO: this smells a lot
            var innerContainer = container.Resolve<Practices.IoC.SimpleContainer>();
            foreach (var simpleModule in simpleModules)
            {
                simpleModule.RegisterModule(innerContainer, () => CurrentLifetimeScope);
            }
        }
    }
}
