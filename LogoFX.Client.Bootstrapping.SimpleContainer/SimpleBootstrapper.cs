using System.Linq;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping.SimpleContainer
{
    /// <summary>
    /// Represents bootstrapper which uses Extended Simple Container adapter
    /// </summary>
    /// <typeparam name="TRootViewModel"></typeparam>
    public class SimpleBootstrapper<TRootViewModel> : BootstrapperContainerBase<TRootViewModel, ExtendedSimpleContainerAdapter> where TRootViewModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleBootstrapper{TRootViewMode}"/> class.
        /// </summary>
        /// <param name="containerAdapter">IoC container adapter</param>
        /// <param name="useApplication">
        /// True if there is an actual WPF application, false otherwise. 
        /// Use false value for tests.
        /// </param>
        /// <param name="reuseCompositionInformation">
        /// True if the composition information should be reused, false otherwise.
        /// Use 'true' to boost the tests. Pay attention to cross-thread calls.</param>
        public SimpleBootstrapper(ExtendedSimpleContainerAdapter containerAdapter, bool useApplication = true, bool reuseCompositionInformation = false)
            :base(containerAdapter, useApplication, reuseCompositionInformation)
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
