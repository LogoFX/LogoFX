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

#if LATER

using System;
using System.ComponentModel;
using System.Reflection;

namespace LogoFX.ViewModels
{
    /// <summary>
    /// todo:make property work like path or introduce depobject
    /// </summary>
    public class PropertyViewModel:ViewModelBase
    {
        private readonly string m_Property;
        private readonly object m_Obj;

        public PropertyViewModel(string property,object obj)
        {
            m_Property = property;
            m_Obj = obj;
            if(m_Obj is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)m_Obj).PropertyChanged+=ObjectPropertyChanged;
            }
        }

        private void ObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(()=>Value);
        }

        public override string DisplayName
        {
            get
            {
                return base.DisplayName??m_Property;
            }
        }


        #region Method Invocation helpers

        public static void CallMethod(object o, string methodName, object[] args)
        {
            CallMethod<object>(o, methodName, args);
        }

        private static T CallMethod<T>(object o, string methodName, object[] args)
        {
            object obj = GetMethod(o, methodName, args).Invoke(o, args);
            return (T)obj;
        }

        private static MethodInfo GetMethod(object o, string methodName, object[] args)
        {
            Type type = o.GetType();
            MethodInfo method = type.GetMethod(methodName) ??
                                type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            return method;
        }

        #endregion

        #region Value property


        public object Value
        {
            get
            {
                try
                {
                    return CallMethod<object>(m_Obj,"get_" + m_Property, null);
                }
                catch
                {
                    return null;
                }
                
                
            }
            set
            {
                object val = Value;
                if (val == value)
                    return;

                object oldValue = val;
                try
                {
                    CallMethod(m_Obj,"set_" + m_Property, new object[]{value});
                    OnValueChangedOverride(value, oldValue);
                    OnPropertyChanged(()=>Value);
                }
                catch
                {
                }
                
            }
        }

        protected virtual void OnValueChangedOverride(object newValue, object oldValue)
        {
        }

        #endregion

        public override void Dispose()
        {
            if (m_Obj is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)m_Obj).PropertyChanged -= ObjectPropertyChanged;
            }
        }
    }
}
#endif