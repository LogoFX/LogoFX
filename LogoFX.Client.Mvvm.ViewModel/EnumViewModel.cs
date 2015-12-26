using System;
using System.Collections.Generic;
using System.Linq;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents a view model which wraps around an enum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumEntryViewModel<T>:ObjectViewModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumEntryViewModel{T}"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public EnumEntryViewModel(T obj):base(obj)
        {
            
        }        
    }

    /// <summary>
    /// Represents a view model which wraps around a collection of enum values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumViewModel<T> : IHierarchicalViewModel
    {
        private ObservableViewModelsCollection<IObjectViewModel> _children;

        /// <summary>
        /// Returns an enum model wrapper for specified enum value.
        /// </summary>
        /// <param name="item">The specified enum value.</param>
        public EnumEntryViewModel<T> this[T item]
        {
#if WinRT
            get { return InternalChildren.Cast<EnumEntryViewModel<T>>().First(a => a.ObjectModel.Equals(item)); }
#else
            get { return InternalChildren.Cast<EnumEntryViewModel<T>>().First(a => a.Model.Equals(item)); }
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumViewModel{T}"/> class.
        /// </summary>
        public EnumViewModel()
        {
            EnumHelper.GetValues<T>().ForEach(a => InternalChildren.Add(new EnumEntryViewModel<T>(a)));
        }

        #region Implementation of IHierarhicalViewModel

        /// <summary>
        /// Gets the children.
        /// </summary>
        public IViewModelsCollection<IObjectViewModel> Children
        {
            get { return InternalChildren; }
        }

        /// <summary>
        /// Gets the items.(GLUE:compatibility to caliburn micro)
        /// </summary>
        public IViewModelsCollection<IObjectViewModel> Items
        {
            get { return InternalChildren; }
        }

        private ObservableViewModelsCollection<IObjectViewModel> InternalChildren
        {
            get { return _children?? (_children = new ObservableViewModelsCollection<IObjectViewModel>()); }
        }

        
        #endregion
    }

    /// <summary>
    /// Helper class for enum operations.
    /// </summary>
    public static class EnumHelper
    {
        private static readonly IDictionary<Type, object[]> s_enumCache = new Dictionary<Type, object[]>();

        /// <summary>
        /// Gets the boxed enum value.
        /// </summary>
        /// <param name="s">The unboxed enum value.</param>
        /// <returns></returns>
        public static object GetBoxed(Enum s)
        {
            Type enumType = s.GetType();
            object ret = GetValues(enumType).Where(ss => ss.ToString() == s.ToString()).FirstOrDefault();
            return ret;
        }

        /// <summary>
        /// Gets all enum values from the specified enum type.
        /// </summary>
        /// <typeparam name="T">The specified enum type.</typeparam>
        /// <returns></returns>
        public static T[] GetValues<T>()
        {
            return GetValues(typeof (T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Gets the unboxed enum values from the specified enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Type ' + enumType.Name + ' is not an enum</exception>
        public static object[] GetValues(Type enumType)
        {
#if WinRT
            if (!enumType.IsEnum())
#else
            if (!enumType.IsEnum)
#endif            
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            object[] values;
            if (!s_enumCache.TryGetValue(enumType, out values))
            {
#if WinRT
                values = (from field in enumType.GetTypeInfo().DeclaredFields
                          where field.IsLiteral
                          select field.GetValue(enumType)).ToArray();
#else
                values = (from field in enumType.GetFields()
                          where field.IsLiteral
                          select field.GetValue(enumType)).ToArray();
#endif
                s_enumCache[enumType] = values;
            }
            return values;
        }
    }
}
