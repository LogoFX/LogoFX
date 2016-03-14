using System.Collections.Generic;
using System.IO;
using System.Linq;
using Solid.Practices.Composition;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    internal static class CompositionHelper
    {        
        internal static IEnumerable<ICompositionModule> GetCompositionModules(
            string modulesPath,
            string[] prefixes,
            bool reuseCompositionInformation)
        {
            var rootPath = Directory.GetCurrentDirectory() + modulesPath;
            ICompositionModule[] compositionModules;
            if (reuseCompositionInformation == false)
            {
                compositionModules = CreateCompositionModules(rootPath, prefixes);
            }
            else
            {
                compositionModules = CompositionStorage.GetCompositionModules(rootPath);
                if (compositionModules != null)
                {
                    return compositionModules;
                }
                compositionModules = CreateCompositionModules(rootPath, prefixes).ToArray();
                CompositionStorage.AddCompositionModules(rootPath, compositionModules);
                return compositionModules;
            }
            return compositionModules;
        }

        private static ICompositionModule[] CreateCompositionModules(
            string modulesPath,
            string[] prefixes)
        {
            var compositionManager = new CompositionManager();
            compositionManager.Initialize(modulesPath, prefixes);
            return compositionManager.Modules.ToArray();
        }
    }
}
