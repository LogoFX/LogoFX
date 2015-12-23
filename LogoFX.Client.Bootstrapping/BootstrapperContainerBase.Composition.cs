using System.Collections.Generic;
using System.Reflection;
using Solid.Practices.Composition;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    partial class BootstrapperContainerBase<TRootViewModel, TIocContainer> : ICompositionModulesProvider
    {
        private readonly bool _reuseCompositionInformation;
        private ICompositionInitializationFacade _compositionInfo;

        public virtual string ModulesPath
        {
            get { return "."; }
        }

        public virtual string[] Prefixes
        {
            get { return new string[] { }; }
        }

        public IEnumerable<ICompositionModule> Modules
        {
            get { return _compositionInfo.Modules; }
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {                       
            return Assemblies;
        }

        private void RegisterCompositionModules(TIocContainer iocContainer)
        {            
            var moduleRegistrator = new ModuleRegistrator(Modules);
            moduleRegistrator.RegisterModules(iocContainer);
        }

        private void InitializeCompositionInfo()
        {
            _compositionInfo = CompositionInfoHelper.GetCompositionInfo(ModulesPath, Prefixes,
                    GetType(),
                    _reuseCompositionInformation);
        }
    }
}
