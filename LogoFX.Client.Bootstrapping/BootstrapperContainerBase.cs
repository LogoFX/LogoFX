using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Core;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{    
    /// <summary>
    /// Base class for application and test boostrappers.
    /// Used when no navigation or special IoC container adapter-dependent logic is needed.
    /// </summary>
    /// <typeparam name="TRootViewModel">Type of Root ViewModel</typeparam>
    /// <typeparam name="TIocContainerAdapter">Type of IoC container adapter</typeparam>
    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> :
#if !WinRT
 BootstrapperBase
#else
        CaliburnApplication
#endif        
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IBootstrapperAdapter, new()
    {        
        private readonly TIocContainerAdapter _iocContainerAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperContainerBase{TRootViewModel, TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">IoC container adapter</param>
        /// <param name="useApplication">
        /// True if there is an actual WPF application, false otherwise. 
        /// Use false value for tests.
        /// </param>
        /// <param name="reuseCompositionInformation">
        /// True if the composition information should be reused, false otherwise.
        /// Use 'true' to boost the tests. Pay attention to cross-thread calls.</param>
        protected BootstrapperContainerBase(
            TIocContainerAdapter iocContainerAdapter, 
            bool useApplication = true, 
            bool reuseCompositionInformation = false)            
            :base(useApplication)
        {
            _iocContainerAdapter = iocContainerAdapter;
            _reuseCompositionInformation = reuseCompositionInformation;
            InitializeCompositionInfo();
            Initialize();
        }

        private void DisplayRootView()
        {
            DisplayRootViewFor(typeof(TRootViewModel));
        }

        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param><param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootView();
        }

        /// <summary>
        /// Configures the framework and executes boiler-plate registrations.
        /// </summary>
        protected sealed override void Configure()
        {
            base.Configure();
            InitializeDispatcher();
            InitializeViewLocator();
            InitializeAdapter();            
            RegisterCommon(_iocContainerAdapter);
            RegisterViewsAndViewModels(_iocContainerAdapter);
            RegisterCompositionModules(_iocContainerAdapter);
            OnConfigure(_iocContainerAdapter);
        }

        private static void InitializeDispatcher()
        {
            Dispatch.Current.InitializeDispatch();
        }

        private static void RegisterCommon(TIocContainerAdapter iocContainerAdapter)
        {
            iocContainerAdapter.RegisterSingleton<IWindowManager, WindowManager>();
            iocContainerAdapter.RegisterSingleton<TRootViewModel, TRootViewModel>();
            iocContainerAdapter.RegisterInstance(iocContainerAdapter);
            iocContainerAdapter.RegisterInstance<IIocContainer>(iocContainerAdapter);
        }

        private void RegisterViewsAndViewModels(IIocContainerRegistrator iocContainer)
        {            
            Assemblies
                .SelectMany(assembly => assembly.GetTypes())         
                .Where(type => type != typeof(TRootViewModel) && type.Name.EndsWith("ViewModel"))                
                .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace != null && type.Namespace.EndsWith("ViewModels"))                
                .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name, false) != null)
                .ForEach(a => iocContainer.RegisterTransient(a, a));            
        }

        private void RegisterCompositionModules(TIocContainerAdapter iocContainerAdapter)
        {
            new ModuleRegistrator(Modules).RegisterModules(iocContainerAdapter);
        }

        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="iocContainerAdapter">IoC container adapter</param>
        protected virtual void OnConfigure(TIocContainerAdapter iocContainerAdapter)
        {
        }

        private readonly object _defaultLifetimeScope = new object();

        /// <summary>
        /// Gets the current lifetime scope.
        /// </summary>
        /// <value>
        /// The current lifetime scope.
        /// </value>
        public virtual object CurrentLifetimeScope
        {
            get { return _defaultLifetimeScope; }
        }

        /// <summary>
        /// Gets the assemblies that will be inspected for the application components.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        protected Assembly[] Assemblies
        {
            get { return _compositionInfo.AssembliesResolver.GetAssemblies().ToArray(); }
        }
    }    
}
