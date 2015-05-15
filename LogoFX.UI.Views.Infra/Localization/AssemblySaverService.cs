using System.IO;
using System.Reflection;

namespace LogoFX.UI.Views.Infra.Localization
{
    public sealed class AssemblySaverService
    {
        #region Fields

        private static AssemblySaverService s_instance;

        private static readonly object s_sync = new object();

        #endregion

        #region Constructors

        private AssemblySaverService()
        {

        }

        #endregion

        #region Public Methods

        public void Save(LocalAssemblyCollection localAssemblyCollection)
        {
            foreach (var pair in localAssemblyCollection)
            {
                Save(localAssemblyCollection.Path, pair.Key, pair.Value);
            }
        }

        #endregion

        #region Public Properties

        public static AssemblySaverService Instance
        {
            get
            {
                lock (s_sync)
                {
                    return s_instance ?? (s_instance = new AssemblySaverService());
                }
            }
        }

        #endregion

        #region Private Members

        private void Save(string path, AssemblyName assemblyName, ResourceSetCollection resourceSetCollection)
        {
            string assemblyFileName = AssemblyResourceUtility.CreateLocalAssemblyFileName(path, assemblyName);

            path = Path.GetDirectoryName(assemblyFileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(assemblyFileName))
            {
                File.Delete(assemblyFileName);
            }

            AssemblyResourceSaver assemblyResourceSaver = AssemblyResourceSaver.CreateInNewDomain(assemblyName, assemblyFileName);
            assemblyResourceSaver.CreateAssembly(resourceSetCollection);
            AssemblyResourceSaver.DestroyDomain(assemblyResourceSaver);
        }

        #endregion
    }
}