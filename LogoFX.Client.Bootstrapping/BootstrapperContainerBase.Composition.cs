using System.Collections.Generic;
using System.Reflection;
using Solid.Practices.Composition;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    partial class BootstrapperContainerBase<TRootViewModel, TIocContainer> : ICompositionModulesProvider
    {
        private readonly bool _reuseCompositionInformation;

        public virtual string ModulesPath
        {
            get { return "."; }
        }

        public virtual string[] Prefixes
        {
            get { return new string[] { }; }
        }

        public IEnumerable<ICompositionModule> Modules { get; private set; }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var compositionInfo =
                CompositionInfoHelper.GetCompositionInfo(ModulesPath, Prefixes,
                    GetType(),
                    _reuseCompositionInformation);
            Modules = compositionInfo.Modules;
            return compositionInfo.AssembliesResolver.GetAssemblies();
        }

        private void RegisterModules()
        {
            var moduleRegistrator = new ModuleRegistrator(Modules);
            moduleRegistrator.RegisterModules(_iocContainer);
        }
    }
}
