﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace LogoFX.Server.Web.Bootstrapping
{
    class HttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        private readonly Predicate<Type> _isControllerTypePredicate = IsControllerType;
        private Func<Assembly, Type[]> _getTypesFunc = GetTypes;
        private IEnumerable<Type> _controllerTypes;

        protected virtual Predicate<Type> IsControllerTypePredicate
        {
            get { return _isControllerTypePredicate; }
        }

        /// <summary>
        /// Returns a list of controllers available for the application.
        /// </summary>
        /// <returns>An <see cref="ICollection{Type}"/> of controllers.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Catching all exceptions in this case is the right to do.")]
        public virtual ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            if (assembliesResolver == null)
            {
                throw new ArgumentNullException("assembliesResolver");
            }

            // Go through all assemblies referenced by the application and search for types matching a predicate
            ICollection<Assembly> assemblies = assembliesResolver.GetAssemblies();

            return (_controllerTypes ?? (_controllerTypes = GetControllerTypesInternal(assemblies))).ToList();
        }

        private IEnumerable<Type> GetControllerTypesInternal(IEnumerable<Assembly> assemblies)
        {
            var result = new List<Type>();

            foreach (Assembly assembly in assemblies)
            {
                Type[] exportedTypes;
                if (assembly == null || assembly.IsDynamic)
                {
                    // can't call GetTypes on a null (or dynamic?) assembly
                    continue;
                }

                try
                {
                    exportedTypes = _getTypesFunc(assembly);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exportedTypes = ex.Types;
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    // We deliberately ignore all exceptions when building the cache. If 
                    // a controller type is not found then we will respond later with a 404.
                    // However, until then we don't know whether an exception at all will
                    // have an impact on finding a controller.
                    continue;
                }

                if (exportedTypes != null)
                {
                    result.AddRange(exportedTypes.Where(x => TypeIsVisible(x) && IsControllerTypePredicate(x)));
                }
            }

            result.ForEach(x => Console.WriteLine(string.Format("Loaded controller: {0}", x.FullName)));

            return result;
        }

        /// <summary>
        /// Returns an array of <see cref="Type"/>s in the given <paramref name="assembly"/>.
        /// </summary>
        /// <remarks>Available separately only to allow a tracing override.</remarks>
        /// <param name="assembly">The <see cref="Assembly"/> to check.</param>
        /// <returns>Array of <see cref="Type"/>s types in the assembly.</returns>
        private static Type[] GetTypes(Assembly assembly)
        {
            return assembly.GetTypes();
        }

        /// <summary>
        /// We match if type name ends with "Controller" and that is not the only part of the 
        /// name (i.e it can't be just "Controller"). The reason is that the route name has to 
        /// be a non-empty prefix of the controller type name.
        /// </summary>
        private static bool HasValidControllerName(Type controllerType)
        {
            Contract.Assert(controllerType != null);
            string controllerSuffix = DefaultHttpControllerSelector.ControllerSuffix;
            return controllerType.Name.Length > controllerSuffix.Length && controllerType.Name.EndsWith(controllerSuffix, StringComparison.OrdinalIgnoreCase);
        }

        internal static bool IsControllerType(Type t)
        {
            return
                t != null &&
                t.IsClass &&
                t.IsVisible &&
                !t.IsAbstract &&
                typeof(IHttpController).IsAssignableFrom(t) &&
                HasValidControllerName(t);
        }

        private static bool TypeIsVisible(Type type)
        {
            return (type != null && type.IsVisible);
        }
    }
}
