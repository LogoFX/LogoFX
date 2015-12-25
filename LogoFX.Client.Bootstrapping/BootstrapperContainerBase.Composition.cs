using System.Collections.Generic;
using System.Reflection;
using Solid.Practices.Composition;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    partial class BootstrapperContainerBase<TRootViewModel, TIocContainer> : ICompositionModulesProvider
    {
        private readonly bool _reuseCompositionInformation;
        private ICompositionInitializationFacade _compositionInfo;

        /// <summary>
        /// Gets the path of composition modules that will be discovered during bootstrapper configuration.
        /// </summary>
        public virtual string ModulesPath
        {
            get { return "."; }
        }

        /// <summary>
        /// Gets the prefixes of the modules that will be used by the module registrator
        /// during bootstrapper configuration. Use this to implement efficient discovery.
        /// </summary>
        /// <value>
        /// The prefixes.
        /// </value>
        public virtual string[] Prefixes
        {
            get { return new string[] { }; }
        }

        /// <summary>
        /// Gets the list of modules that were discovered during bootstrapper configuration.
        /// </summary>
        /// <value>
        /// The list of modules.
        /// </value>
        public IEnumerable<ICompositionModule> Modules
        {
            get { return _compositionInfo.Modules; }
        }

        /// <summary>
        /// Override to tell the framework where to find assemblies to inspect for application components.
        /// </summary>
        /// <returns>
        /// A list of assemblies to inspect.
        /// </returns>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {                       
            return Assemblies;
        }

        private void InitializeCompositionInfo()
        {
            _compositionInfo = CompositionInfoHelper.GetCompositionInfo(ModulesPath, Prefixes,
                    GetType(),
                    _reuseCompositionInformation);
        }        
    }
}
