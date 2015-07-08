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

namespace LogoFX.Client.Mvvm.View.Infra.Interactivity.Behaviors
{
    /// <summary>
    /// Provides functionality missed in Silverlight :
    /// {Binding UpdateSourceTrigger=PropertyChange}    
    /// </summary>
    public class UpdateSourceOnChangeBehavior : Behavior<DependencyObject>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            var txt = AssociatedObject as TextBox;
            if (txt != null)
            {
                txt.TextChanged += OnTextChanged;
                return;
            }
#if SILVERLIGHT
            var pass = AssociatedObject as PasswordBox;
            if (pass != null)
            {
                pass.PasswordChanged += OnPasswordChanged;
                return;
            }
#endif
        }

        protected override void OnDetaching()
        {
            var txt = AssociatedObject as TextBox;
            if (txt != null)
            {
                txt.TextChanged -= OnTextChanged;
                return;
            }
#if SILVERLIGHT
            var pass = AssociatedObject as PasswordBox;
            if (pass != null)
            {
                pass.PasswordChanged -= OnPasswordChanged;
                return;
            }
#endif
            base.OnDetaching();
        }
#if SILVERLIGHT
        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var txt = sender as PasswordBox;
            if (txt == null)
                return;
            var be = txt.GetBindingExpression(PasswordBox.PasswordProperty);
            if (be != null)
            {
                be.UpdateSource();
            }
        }
#endif
        static void OnTextChanged(object sender,
          TextChangedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt == null)
                return;
            var be = txt.GetBindingExpression(TextBox.TextProperty);
            if (be != null)
            {
                be.UpdateSource();
            }
        }

    }
}
