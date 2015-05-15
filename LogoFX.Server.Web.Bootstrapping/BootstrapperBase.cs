using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http.Dispatcher;
using LogoFX.Server.Web.Core;
using Solid.Practices.Composition.Web;
using Solid.Practices.IoC;

namespace LogoFX.Server.Web.Bootstrapping
{
    public abstract class BootstrapperBase
    {
        private bool _isInitialized;       
        private readonly IIocContainer _iocContainer;       
        private IAssembliesResolver _assembliesResolver;
        private IHttpControllerTypeResolver _typeResolver;
        private IHttpControllerActivator _controllerActivator;
        private object _defaultLifetimeScope;        

        protected BootstrapperBase(IIocContainer iocContainer)
            : this(HttpControllerTypeResolver.IsControllerType)
        {            
            _iocContainer = iocContainer;
            Start();
        }

        private BootstrapperBase(Predicate<Type> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }            
        }

        private void Start()
        {
            if (_isInitialized)
            {
                return;
            }
            _isInitialized = true;           
            StartRuntime();
        }        
        
        public virtual string ModulesPath
        {
            get { return string.Empty; }
        }

        protected void SetupHttpConfiguration(IHttpConfigurationProxy config)
        {            
            config.MapHttpRoutes();
            config.ReplaceService(_assembliesResolver);
            config.ReplaceService(_typeResolver);
            config.ReplaceService(_controllerActivator);            
        }

        /// <summary>
        /// Gets the current lifetime scope.
        /// </summary>
        /// <value>The current lifetime scope.</value>
        protected virtual object CurrentLifetimeScope
        {
            get { return _defaultLifetimeScope ?? (_defaultLifetimeScope = new Object()); }
        }

        private void StartRuntime()
        {            
            DirectoryInfo directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string path = Path.Combine(directoryInfo.FullName, ModulesPath);
            var initializationFacade = new BootstrapperInitializationFacade<IIocContainer>(_iocContainer);  
            initializationFacade.Initialize(path);
            //code smell
            _assembliesResolver = initializationFacade.AssembliesResolver as IAssembliesResolver;
            _typeResolver = new HttpControllerTypeResolver();
            _controllerActivator = new HttpControllerActivator(_iocContainer);
            RegisterServices();              
            Configure();
            RegisterControllers(_typeResolver.GetControllerTypes(_assembliesResolver));            
        }

        protected abstract void Configure();

        private void RegisterServices()
        {
            _iocContainer.RegisterInstance(_iocContainer);
            _iocContainer.RegisterInstance(_assembliesResolver);
            _iocContainer.RegisterInstance(_typeResolver);
            _iocContainer.RegisterInstance(_controllerActivator);            
        }

        private void RegisterControllers(IEnumerable<Type> controllerTypes)
        {
            foreach (var controllerType in controllerTypes)
            {
                _iocContainer.RegisterTransient(controllerType, controllerType);
            }            
        }                      
    }    
}