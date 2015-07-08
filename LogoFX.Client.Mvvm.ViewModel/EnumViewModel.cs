// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// <email>mailto:vlads@logoui.co.il</email>
// <created>21/00/10</created>
// <lastedit>21/00/10</lastedit>

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the 'Software'), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//
// <remarks>Part of this software based on various internet sources, mostly on the works
// of members of Wpf Disciples group http://wpfdisciples.wordpress.com/
// Also project may contain code from the frameworks: 
//        Nito 
//        OpenLightGroup
//        nRoute
// </remarks>
// ====================================================================================//

using System;
using System.Collections.Generic;
using System.Linq;
using LogoFX.Core;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;

namespace LogoFX.Client.Mvvm.ViewModel
{
    public class EnumEntryViewModel<T>:ObjectViewModel<T>
    {
        public EnumEntryViewModel(T obj):base(obj)
        {
            
        }        
    }

    public class EnumViewModel<T> : IHierarchicalViewModel
    {
        private ObservableViewModelsCollection<IObjectViewModel> _children;

        public EnumEntryViewModel<T> this[T item]
        {
#if WinRT
            get { return InternalChildren.Cast<EnumEntryViewModel<T>>().First(a => a.ObjectModel.Equals(item)); }
#else
            get { return InternalChildren.Cast<EnumEntryViewModel<T>>().First(a => a.Model.Equals(item)); }
#endif
        }

        public EnumViewModel()
        {
            EnumHelper.GetValues<T>().ForEach(a => InternalChildren.Add(new EnumEntryViewModel<T>(a)));
        }

        #region Implementation of IHierarhicalViewModel

        public IViewModelsCollection<IObjectViewModel> Children
        {
            get { return InternalChildren; }
        }

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


    public static class EnumHelper
    {
        private static readonly IDictionary<Type, object[]> s_enumCache = new Dictionary<Type, object[]>();

        public static object GetBoxed(System.Enum s)
        {
            Type enumType = s.GetType();
            object ret = GetValues(enumType).Where(ss => ss.ToString() == s.ToString()).FirstOrDefault();
            return ret;
        }

        public static T[] GetValues<T>()
        {
            return GetValues(typeof (T)).Cast<T>().ToArray();
        }

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
