using System.Collections.Concurrent;
using Solid.Practices.Composition;

namespace LogoFX.Client.Bootstrapping
{
    static class CompositionInfoStorage
    {
        private static readonly ConcurrentDictionary<string, ICompositionInitializationInfo> _internalStorage =
            new ConcurrentDictionary<string, ICompositionInitializationInfo>();

        internal static void AddCompositionInfo(string rootPath, ICompositionInitializationInfo compositionInfo)
        {
            _internalStorage.TryAdd(rootPath, compositionInfo);
        }

        internal static ICompositionInitializationInfo GetCompositionInfo(string rootPath)
        {
            ICompositionInitializationInfo compositionInfo;
            _internalStorage.TryGetValue(rootPath, out compositionInfo);
            return compositionInfo;            
        }
    }
}
