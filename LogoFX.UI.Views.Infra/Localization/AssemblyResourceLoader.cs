using System;
using System.Reflection;

namespace LogoFX.UI.Views.Infra.Localization
{
    public sealed class AssemblyResourceLoader : AssemblyResourceHelperBase
    {
        #region Fields

        private readonly Assembly _assembly;

        #endregion

        #region Constructors

        public AssemblyResourceLoader(AssemblyName assemblyName)
            : base(assemblyName)
        {
            try
            {
                _assembly = Assembly.Load(assemblyName);
            }

            catch (Exception)
            {
                _assembly = null;
            }
        }

        #endregion

        #region Public Methods

        public static AssemblyResourceLoader CreateInNewDomain(AssemblyName assemblyName)
        {
            return CreateInNewDomainInternal<AssemblyResourceLoader>(assemblyName);
        }

        public ResourceSetCollection ExtractResources()
        {
            ResourceSetCollection result = new ResourceSetCollection();

            if (ReferenceEquals(_assembly, null))
            {
                return result;
            }

            result = new WinRes().EnumStringResources(_assembly.Location) ?? // read resources from Win32 DLL.
                     AssemblyResourceUtility.ExtractResources(_assembly);    // read resources from managed assembly.

            return result;
        }

        #endregion
    }
}
