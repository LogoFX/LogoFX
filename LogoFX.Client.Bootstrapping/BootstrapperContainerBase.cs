using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping.Contracts;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Desktop;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{    
    /// <summary>
    /// Base class for application and test boostrappers.
    /// Used when no navigation or special IoC containers are neeeded.
    /// </summary>
    /// <typeparam name="TRootViewModel">Type of Root ViewModel</typeparam>
    /// <typeparam name="TIocContainer">Type of IoC container</typeparam>
    public class BootstrapperContainerBase<TRootViewModel, TIocContainer> :
#if !WinRT
 BootstrapperBase
#else
        CaliburnApplication
#endif
        , ICompositionModulesProvider
        where TRootViewModel : class
        where TIocContainer : class, IIocContainer, IBootstrapperAdapter, new()
    {
        private readonly Dictionary<string, Type> _typedic = new Dictionary<string, Type>();
        private IBootstrapperAdapter _bootstrapperAdapter;        
        private readonly TIocContainer _iocContainer;

        /// <summary>
        /// This ctor is used when the container is not created outside the bootstrapper.
        /// This approach is not recommended.
        /// </summary>
        /// <param name="useApplication">
        /// True if there is an actual WPF application, false otherwise. 
        /// Use false value for tests.
        /// </param>
        protected BootstrapperContainerBase(bool useApplication = true)
            : this(new TIocContainer(), useApplication)
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
        protected BootstrapperContainerBase(TIocContainer iocContainer, bool useApplication=true)            
            :base(useApplication)
        {
            _iocContainer = iocContainer;
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
            Dispatch.Current.InitializeDispatch();
            //overriden for performance reasons (Assembly caching)
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {                
                var viewTypeName = modelType.FullName.Substring(0, modelType.FullName.IndexOf("`") < 0
                    ? modelType.FullName.Length
                    : modelType.FullName.IndexOf("`")
                    ).Replace("Model", string.Empty);

                if (context != null)
                {
                    viewTypeName = viewTypeName.Remove(viewTypeName.Length - 4, 4);
                    viewTypeName = viewTypeName + "." + context;
                }

                Type viewType;
                if (!_typedic.TryGetValue(viewTypeName, out viewType))
                {
                    _typedic[viewTypeName] = viewType = (from assembly in AssemblySource.Instance
                                                         from type in assembly.GetExportedTypes()
                                                         where type.FullName == viewTypeName
                                                         select type).FirstOrDefault();
                }

                return viewType;
            };
            ViewLocator.LocateForModelType = (modelType, displayLocation, context) =>
            {
                var viewType = ViewLocator.LocateTypeForModelType(modelType, displayLocation, context);

                return viewType == null
                    ? new TextBlock { Text = string.Format("Cannot find view for\nModel: {0}\nContext: {1} .", modelType, context) }
                    : ViewLocator.GetOrCreateViewType(viewType);
            };
            _bootstrapperAdapter = _iocContainer;

            RegisterCommon(_iocContainer);
            RegisterViewsAndViewModels(_iocContainer);
            var moduleRegistrator = new ModuleRegistrator(Modules);
            moduleRegistrator.RegisterModules(_iocContainer);
            OnConfigure(_iocContainer);
        }        

        private static void RegisterCommon(IIocContainer iocContainer)
        {
            iocContainer.RegisterSingleton<IWindowManager, WindowManager>();
            iocContainer.RegisterSingleton<TRootViewModel, TRootViewModel>();
            iocContainer.RegisterInstance(iocContainer);
        }
        
        protected virtual void OnConfigure(TIocContainer container)
        {
        }

        protected override object GetInstance(Type service, string key)
        {
            return _bootstrapperAdapter.GetInstance(service);
        }
        
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _bootstrapperAdapter.GetAllInstances(service);
        }
        
        protected override void BuildUp(object instance)
        {
            _bootstrapperAdapter.BuildUp(instance);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {                     
            var initializationFacade = new CompositionInitializationFacade(GetType());
            initializationFacade.Initialize(ModulesPath,Prefixes);
            Modules = initializationFacade.Modules;
            return initializationFacade.AssembliesResolver.GetAssemblies();
        }

        public virtual string ModulesPath
        {
            get { return "."; }
        }

        public virtual string[] Prefixes
        {
            get { return new string[]{}; }
        }

        private readonly object _defaultLifetimeScope = new object();
        public virtual object CurrentLifetimeScope
        {
            get { return _defaultLifetimeScope; }
        }

        public IEnumerable<ICompositionModule> Modules { get; private set; }

        private static void RegisterViewsAndViewModels(IIocContainerRegistrator iocContainer)
        {            
            AssemblySource.Instance.ToArray()
              .SelectMany(ass => ass.GetTypes())         
              .Where(type => type != typeof(TRootViewModel) && type.Name.EndsWith("ViewModel"))                
              .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace != null && type.Namespace.EndsWith("ViewModels"))                
              .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name, false) != null)
              .Apply(a => iocContainer.RegisterTransient(a, a));            
        }
    }    
}
