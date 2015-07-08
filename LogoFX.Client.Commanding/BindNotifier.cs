// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>Vlad Spivak</author>
// <email>mailto:vlads@logoui.co.il</email>
// <created>20/00/10</created>
// <lastedit>20/00/10</lastedit>

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
#if !WinRT
using System.Windows;
using System.Windows.Data;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace LogoFX.Client.Commanding
{
    /// <summary>
    /// <see cref="DependencyObject"/> based notifier on property change 
    /// </summary>
    public class NotificationHelperDp : DependencyObject
    {
        private readonly Action<object, object> _callback;
        private bool _isDetached = false;

        public NotificationHelperDp(Action<object, object> callback)
        {
            _callback = callback;
        }

        public object BindValue
        {
            get { return (object) GetValue(BindValueProperty); }
            set { SetValue(BindValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindValueProperty =
            DependencyProperty.Register("BindValue", typeof (object), typeof (NotificationHelperDp),
                new PropertyMetadata(null, OnBindValueChanged));

        private static void OnBindValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotificationHelperDp that = (NotificationHelperDp) d;

            if (!that._isDetached && that._callback != null
#if !SILVERLIGHT && !WinRT
                && BindingOperations.IsDataBound(that, BindValueProperty)
#endif
                )
                that._callback(e.NewValue, e.OldValue);
        }

        public void Detach()
        {
            _isDetached = true;
            this.ClearValue(BindValueProperty);
        }
    }

    public static class BindNotifier
    {
        static readonly WeakKeyDictionary<object, Dictionary<string, NotificationHelperDp>> _notifiers = new WeakKeyDictionary<object, Dictionary<string, NotificationHelperDp>>();
        public static void NotifyOn<T>(this T vmb, string path, Action<object, object> callback)
        {
            Dictionary<string, NotificationHelperDp> block;
            if (!_notifiers.TryGetValue(vmb, out block))
            {
                _notifiers.Add(vmb, block = new Dictionary<string, NotificationHelperDp>());
            }
            block.Remove(path);

            NotificationHelperDp binder = new NotificationHelperDp(callback);
            BindingOperations.SetBinding(binder, NotificationHelperDp.BindValueProperty,
#if !WinRT
 new Binding(path) { Source = vmb });
#else
            new Binding() { Source = vmb,Path = new PropertyPath(path)});
#endif
            block.Add(path, binder);
        }

        public static void UnNotifyOn<T>(this T vmb, string path)
        {
            Dictionary<string, NotificationHelperDp> block;
            if (!_notifiers.TryGetValue(vmb, out block) || !block.ContainsKey(path))
            {
                return;
            }

            block[path].Detach();
            block.Remove(path);
        }
    }
}

