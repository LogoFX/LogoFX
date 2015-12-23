using System;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Desktop;

namespace LogoFX.Client.Bootstrapping
{
    internal static class CompositionInfoHelper
    {
        internal static ICompositionInitializationFacade GetCompositionInfo(string modulesPath, string[] prefixes, Type entryType, bool reuseCompositionInformation)
        {
            var rootPath = Environment.CurrentDirectory + modulesPath;
            ICompositionInitializationFacade compositionInfo;
            if (reuseCompositionInformation == false)
            {
                compositionInfo = CreateCompositionInfo(modulesPath, prefixes, entryType);
            }
            else
            {
                compositionInfo = CompositionInfoStorage.GetCompositionInfo(rootPath);
                if (compositionInfo != null)
                {
                    return compositionInfo;
                }
                compositionInfo = CreateCompositionInfo(modulesPath, prefixes, entryType);
                CompositionInfoStorage.AddCompositionInfo(rootPath, compositionInfo);
                return compositionInfo;
            }            
            return compositionInfo;
        }

        private static CompositionInitializationFacade CreateCompositionInfo(
            string modulesPath, 
            string[] prefixes,
            Type entryType)
        {
            var compositionInfo = new CompositionInitializationFacade(entryType);
            compositionInfo.Initialize(modulesPath, prefixes);           
            return compositionInfo;
        }
    }
}
