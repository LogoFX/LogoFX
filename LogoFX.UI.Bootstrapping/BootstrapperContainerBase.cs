using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using LogoFX.UI.Bootstraping.Contracts;
using Solid.Practices.Composition.Desktop;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.UI.Bootstrapping
{    
    public class BootstrapperContainerBase<TRootViewModel, TIocContainer> : 
        BootstrapperBase<TRootViewModel> 
        where TRootViewModel : class
        where TIocContainer : IIocContainer, IBootstrapperAdapter, new()
    {
        private IBootstrapperAdapter _bootstrapperAdapter;
        private object _defaultLifetimeScope;
        private TIocContainer _iocContainer;

        /// <summary>
        /// Configures this instance.
        /// </summary>
        protected override void Configure()
        {
            base.Configure();
            _bootstrapperAdapter = _iocContainer;

            RegisterCommon(_iocContainer);
            RegisterViewsAndViewModels(_iocContainer);
            OnConfigure(_iocContainer);
        }

        private void RegisterCommon(IIocContainer iocContainer)
        {
            iocContainer.RegisterSingleton<IWindowManager, WindowManager>();
            iocContainer.RegisterSingleton<TRootViewModel, TRootViewModel>();
            iocContainer.RegisterInstance(iocContainer);
        }

        /// <summary>
        /// Called on configure.
        /// </summary>
        /// <param name="container">The container.</param>
        protected virtual void OnConfigure(TIocContainer container)
        {
        }

        #region Overrides

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
            _iocContainer = new TIocContainer();
            var initializationFacade = new BootstrapperInitializationFacade<TIocContainer>(GetType(), _iocContainer);
            initializationFacade.Initialize(ModulesPath);
            Modules = initializationFacade.Modules;
            return initializationFacade.AssembliesResolver.GetAssemblies();
        }

        #endregion

        /// <summary>
        /// Gets the modules path that MEF need to search.
        /// </summary>
        /// <value>The modules path.</value>
        public virtual string ModulesPath
        {
            get { return "."; }
        }

        public IEnumerable<ICompositionModule> Modules { get; private set; }

        #region private implementation
        private void RegisterViewsAndViewModels(IIocContainer iocContainer)
        {
            //  register view models
            AssemblySource.Instance.ToArray()
              .SelectMany(ass => ass.GetTypes())
                //  must be a type that ends with ViewModel
              .Where(type => type != typeof(TRootViewModel) && type.Name.EndsWith("ViewModel"))
                //  must be in a namespace ending with ViewModels
              .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace != null && type.Namespace.EndsWith("ViewModels"))
                //  must implement INotifyPropertyChanged (deriving from PropertyChangedBase will statisfy this)
              .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name, false) != null)
                //  registered as self
              .Apply(a => iocContainer.RegisterTransient(a, a));

            ////  register views
            //AssemblySource.Instance.ToArray()
            //  .SelectMany(ass => ass.GetTypes())
            //    //  must be a type that ends with View
            //  .Where(type => type.Name.EndsWith("View"))
            //    //  must be in a namespace ending with ViewModels
            //  .Where(type => !(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace != null && type.Namespace.EndsWith("Views"))
            //    //  registered as self
            //  .Apply(a => _iocContainer.RegisterPerRequest(a, null, a));

        }
        #endregion
    }    
}
