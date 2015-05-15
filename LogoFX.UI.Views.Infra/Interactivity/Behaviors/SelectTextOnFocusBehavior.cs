// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>LogoUI Team</author>
// <email>mailto:info@logoui.co.il</email>
// <created>21/00/10</created>
// <lastedit>Sunday, November 21, 2010</lastedit>

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


using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
#if !WinRT

#else
using LogoFX.Core;
using Windows.UI.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using EventTrigger = Windows.UI.Interactivity.EventTrigger;
#endif

namespace LogoFX.UI.Views.Infra.Interactivity.Behaviors
{
    /// <summary>
    /// Selects text on focus
    /// </summary>
    public class SelectTextOnFocusBehavior
        : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.GotFocus += AssociatedObjectGotFocus;
#if !SILVERLIGHT && !WinRT
            this.AssociatedObject.GotKeyboardFocus += AssociatedObjectGotFocus;
#endif

        }

        void AssociatedObjectGotFocus(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject is TextBox)
                ((TextBox)this.AssociatedObject).SelectAll();
            else if (AssociatedObject is PasswordBox)
                ((PasswordBox)this.AssociatedObject).SelectAll();

        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.GotFocus -= AssociatedObjectGotFocus;

        }
    }
}
