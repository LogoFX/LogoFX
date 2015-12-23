using System.Collections.Concurrent;
using Solid.Practices.Composition;

namespace LogoFX.Client.Bootstrapping
{
    static class CompositionInfoStorage
    {
        private static readonly ConcurrentDictionary<string, ICompositionInitializationFacade> _internalStorage =
            new ConcurrentDictionary<string, ICompositionInitializationFacade>();

        internal static void AddCompositionInfo(string rootPath, ICompositionInitializationFacade compositionInfo)
        {
            _internalStorage.TryAdd(rootPath, compositionInfo);
        }

        internal static ICompositionInitializationFacade GetCompositionInfo(string rootPath)
        {
            ICompositionInitializationFacade compositionInfo;
            _internalStorage.TryGetValue(rootPath, out compositionInfo);
            return compositionInfo;            
        }
    }
}
