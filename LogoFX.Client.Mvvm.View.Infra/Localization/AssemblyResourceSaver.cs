using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;

namespace LogoFX.Client.Mvvm.View.Infra.Localization
{
    public sealed class AssemblyResourceSaver : AssemblyResourceHelperBase
    {
        #region Fields

        private readonly string _assemblyFile;

        #endregion

        #region Constructors

        public AssemblyResourceSaver(AssemblyName assemblyName, string assemblyFile)
            : base(assemblyName)
        {
            _assemblyFile = assemblyFile;
        }

        #endregion

        #region Dependency Properties

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public static AssemblyResourceSaver CreateInNewDomain(AssemblyName assemblyName, string assemblyFile)
        {
            AssemblyResourceSaver result = CreateInNewDomainInternal<AssemblyResourceSaver>(assemblyName, assemblyFile);

            return result;
        }

        public void CreateAssembly(ResourceSetCollection resourceSetCollection)
        {
            string dir = Path.GetDirectoryName(_assemblyFile);
            string fileName = Path.GetFileName(_assemblyFile);

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.RunAndSave, dir);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(_assemblyName.Name, fileName);

            foreach (string resourceSetName in resourceSetCollection.Keys)
            {
                ResourceCollection resourceCollection = resourceSetCollection[resourceSetName];
                CreateResourceSet(resourceSetName, resourceCollection, moduleBuilder);
            }

            assemblyBuilder.Save(fileName);
        }

        #endregion

        #region Private Members

        private void CreateResourceSet(string name, ResourceCollection resourceCollection, ModuleBuilder moduleBuilder)
        {
            IResourceWriter rw = moduleBuilder.DefineResource(name, "");

            foreach (var pair in resourceCollection)
            {
                rw.AddResource(pair.Key, pair.Value);
            }
        }

        #endregion

    }
}
