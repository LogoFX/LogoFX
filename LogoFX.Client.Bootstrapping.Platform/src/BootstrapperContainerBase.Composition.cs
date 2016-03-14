using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Client;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> : ICompositionModulesProvider
    {
        private readonly bool _reuseCompositionInformation;        

        /// <summary>
        /// Gets the path of composition modules that will be discovered during bootstrapper configuration.
        /// </summary>
        public
#if NET45
            virtual 
#endif
            string ModulesPath
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
        public IEnumerable<ICompositionModule> Modules { get; private set; }

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

        /// <summary>
        /// Override this to provide custom assembly namespaces collection.
        /// </summary>
        protected virtual void OnConfigureAssemblyResolution()
        {
        }

        private void InitializeCompositionModules()
        {
            Modules = CompositionHelper.GetCompositionModules(ModulesPath, Prefixes,
                    _reuseCompositionInformation);
        }

        private void InitializeInspectedAssemblies()
        {
            Assemblies = GetAssemblies();
        }

        private Assembly[] GetAssemblies()
        {
            OnConfigureAssemblyResolution();
            var assembliesResolver = new AssembliesResolver(GetType(),
                new ClientAssemblySourceProvider(Directory.GetCurrentDirectory()));
            return ((IAssembliesReadOnlyResolver)assembliesResolver).GetAssemblies().ToArray();
        }
    }
}
