using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Dispatcher;
using LogoFX.Practices.Composition;

namespace LogoFX.Web.Core
{
    class AssembliesResolver2 : IAssembliesResolver
    {
        private readonly ICompositionModulesProvider _compositionModulesProvider;
        private List<Assembly> _assemblies;

        public AssembliesResolver2(ICompositionModulesProvider compositionModulesProvider)
        {
            _compositionModulesProvider = compositionModulesProvider;            
        }

        public ICollection<Assembly> GetAssemblies()
        {
            return (_assemblies ?? (_assemblies = SelectAssemblies().ToList()));
        }

        private IEnumerable<Assembly> SelectAssemblies()
        {
            var assemblies = new List<Assembly>
            {
                GetEntryAssembly()
            };

            return _compositionModulesProvider.Modules != null
                ? assemblies.Concat(_compositionModulesProvider.Modules.Select(a => a.GetType().Assembly)).Distinct().ToList()
                : assemblies;
        }

        static private Assembly GetEntryAssembly()
        {
            if (HttpContext.Current == null ||
                HttpContext.Current.ApplicationInstance == null)
            {
                return null;
            }

            var type = HttpContext.Current.ApplicationInstance.GetType();
            while (type != null && type.BaseType != null && type.BaseType.Name != "HttpApplication")
            {
                type = type.BaseType;
            }

            return type == null ? null : type.Assembly;
        }
    }
}
