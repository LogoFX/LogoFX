using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Core;

namespace LogoFX.Client.Bootstrapping.SimpleContainer
{
    public class SimpleBootstrapper<TRootViewModel> : BootstrapperContainerBase<TRootViewModel, ExtendedSimpleIocContainer> 
        where TRootViewModel : class
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
            var middlewares = new[] {new SimpleModuleMiddleware(Modules, () => CurrentLifetimeScope)};
            middlewares.ForEach(t => t.Apply(container));
        }
    }
}
