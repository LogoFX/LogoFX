using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.UI.Views.Infra.Localization
{
    public abstract class AssemblyResourceHelperBase : MarshalByRefObject
    {
        #region Fields

        protected readonly AssemblyName _assemblyName;

        private static readonly Dictionary<AssemblyResourceHelperBase, AppDomain> s_domains =
            new Dictionary<AssemblyResourceHelperBase, AppDomain>();
        
        #endregion

        #region Constructors

        protected AssemblyResourceHelperBase(AssemblyName assemblyName)
        {
            _assemblyName = assemblyName;
        }

        #endregion

        #region Public Methods

        protected static T CreateInNewDomainInternal<T>(params object[] args)
            where T : AssemblyResourceHelperBase
        {
            AppDomain domain = AppDomain.CreateDomain("Temp Assembly Domain");
            Type type = typeof(T);
            string typeAssemblyName = type.Assembly.GetName().Name;
            string typeName = type.FullName;
            T instance = (T) domain.CreateInstanceAndUnwrap(typeAssemblyName,
                                                            typeName,
                                                            false,
                                                            BindingFlags.Default,
                                                            null,
                                                            args,
                                                            null,
                                                            null);

            lock (s_domains)
            {
                s_domains.Add(instance, domain);
            }

            return instance;
        }

        public static void DestroyDomain(AssemblyResourceHelperBase assemblyResourceLoader)
        {
            AppDomain domain;
            
            lock (s_domains)
            {
                domain = s_domains[assemblyResourceLoader];
            }

            AppDomain.Unload(domain);
        }

        #endregion
    }
}