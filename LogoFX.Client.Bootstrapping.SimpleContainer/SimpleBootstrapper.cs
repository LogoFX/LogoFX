using System.Linq;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping.SimpleContainer
{
    /// <summary>
    /// Represents bootstrapper which uses Extended Simple Container adapter.
    /// </summary>
    /// <typeparam name="TRootViewModel"></typeparam>
    public class SimpleBootstrapper<TRootViewModel> : 
        BootstrapperContainerBase<TRootViewModel, ExtendedSimpleContainerAdapter> where TRootViewModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleBootstrapper{TRootViewModel}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>        
        public SimpleBootstrapper(ExtendedSimpleContainerAdapter iocContainerAdapter)
            :this(iocContainerAdapter, new BootstrapperCreationOptions())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleBootstrapper{TRootViewModel}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="creationOptions">The creation options.</param>
        public SimpleBootstrapper(
            ExtendedSimpleContainerAdapter iocContainerAdapter,
            BootstrapperCreationOptions creationOptions) :
            base(iocContainerAdapter, creationOptions)
        {
            
        }

        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="iocContainerAdapter">IoC container</param>
        protected override void OnConfigure(ExtendedSimpleContainerAdapter iocContainerAdapter)
        {
            base.OnConfigure(iocContainerAdapter);            
            var simpleModules = Modules.OfType<ISimpleModule>();
            //TODO: this smells a lot
            var innerContainer = iocContainerAdapter.Resolve<Practices.IoC.SimpleContainer>();
            foreach (var simpleModule in simpleModules)
            {
                simpleModule.RegisterModule(innerContainer, () => CurrentLifetimeScope);
            }
        }
    }
}
