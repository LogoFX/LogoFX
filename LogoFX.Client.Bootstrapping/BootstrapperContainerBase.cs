﻿using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping.Contracts;
using LogoFX.Core;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping
{    
    /// <summary>
    /// Base class for application and test boostrappers.
    /// Used when no navigation or special IoC containers are neeeded.
    /// </summary>
    /// <typeparam name="TRootViewModel">Type of Root ViewModel</typeparam>
    /// <typeparam name="TIocContainer">Type of IoC container</typeparam>
    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainer> :
#if !WinRT
 BootstrapperBase
#else
        CaliburnApplication
#endif        
        where TRootViewModel : class
        where TIocContainer : class, IIocContainer, IBootstrapperAdapter, new()
    {        
        private readonly TIocContainer _iocContainer;        

        /// <summary>
        /// This ctor is used when the container is not created outside the bootstrapper.
        /// This approach is not recommended.
        /// </summary>
        /// <param name="useApplication">
        /// True if there is an actual WPF application, false otherwise. 
        /// Use false value for tests.
        /// </param>
        /// <param name="reuseCompositionInformation">
        /// True if the composition information should be reused, false otherwise.
        /// Use 'true' to boost the tests. Pay attention to cross-thread calls.</param>
        protected BootstrapperContainerBase(bool useApplication = true, bool reuseCompositionInformation = false)
            : this(new TIocContainer(), useApplication, reuseCompositionInformation)
        {
           
        }

        /// <summary>
        /// This is the recommended ctor.
        /// </summary>
        /// <param name="iocContainer">IoC container</param>
        /// <param name="useApplication">
        /// True if there is an actual WPF application, false otherwise. 
        /// Use false value for tests.
        /// </param>
        /// <param name="reuseCompositionInformation">
        /// True if the composition information should be reused, false otherwise.
        /// Use 'true' to boost the tests. Pay attention to cross-thread calls.</param>
        protected BootstrapperContainerBase(
            TIocContainer iocContainer, 
            bool useApplication = true, 
            bool reuseCompositionInformation = false)            
            :base(useApplication)
        {
            _iocContainer = iocContainer;
            _reuseCompositionInformation = reuseCompositionInformation;
            InitializeCompositionInfo();
            Initialize();
        }

        private void DisplayRootView()
        {
            DisplayRootViewFor(typeof(TRootViewModel));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootView();
        }
        
        protected override void Configure()
        {
            base.Configure();
            InitializeDispatcher();
            InitializeViewLocator();
            InitializeAdapter();            
            RegisterCommon(_iocContainer);
            RegisterViewsAndViewModels(_iocContainer);
            RegisterCompositionModules(_iocContainer);
            OnConfigure(_iocContainer);
        }

        private static void InitializeDispatcher()
        {
            Dispatch.Current.InitializeDispatch();
        }

        private static void RegisterCommon(IIocContainer iocContainer)
        {
            iocContainer.RegisterSingleton<IWindowManager, WindowManager>();
            iocContainer.RegisterSingleton<TRootViewModel, TRootViewModel>();
            iocContainer.RegisterInstance(iocContainer);
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

        protected virtual void OnConfigure(TIocContainer container)
        {
        }

        private readonly object _defaultLifetimeScope = new object();

        public virtual object CurrentLifetimeScope
        {
            get { return _defaultLifetimeScope; }
        }

        protected Assembly[] Assemblies
        {
            get { return _compositionInfo.AssembliesResolver.GetAssemblies().ToArray(); }
        }
    }    
}
