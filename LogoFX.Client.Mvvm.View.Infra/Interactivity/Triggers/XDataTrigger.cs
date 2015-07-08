// ===================================
// <copyright>LogoUI Co.</copyright>
// <author>LogoUI Team</author>
// <email>mailto:info@logoui.co.il</email>
// <created>25/00/10</created>
// <lastedit>Thursday, November 25, 2010</lastedit>

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
//        Nito - http://nitokitchensink.codeplex.com/
//        OpenLightGroup - http://openlightgroup.net/
//        nRoute - http://nroute.codeplex.com/
//        SilverlightFX - http://projects.nikhilk.net/SilverlightFX
//        BlackLight - http://blacklight.codeplex.com/
// </remarks>
// ====================================================================================//


#if WinRT
#else
using System.Windows;

namespace LogoFX.Client.Mvvm.View.Infra.Interactivity.Triggers
{
    public class XDataTrigger: Microsoft.Expression.Interactivity.Core.DataTrigger
    {

        #region RespectLoadedEvent dependency property

        public bool RespectLoadedEvent
        {
            get { return (bool) GetValue(RespectLoadedEventProperty); }
            set { SetValue(RespectLoadedEventProperty, value); }
        }

        public static readonly DependencyProperty RespectLoadedEventProperty =
            DependencyProperty.Register("RespectLoadedEvent", typeof (bool), typeof (XDataTrigger), new PropertyMetadata(default(bool), OnRespectLoadedEventChanged));

        private static void OnRespectLoadedEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #endregion

        protected override void OnAttached()
        {   
            if(AssociatedObject is FrameworkElement && RespectLoadedEvent)
                ((FrameworkElement)AssociatedObject).Loaded += XDataTriggerLoaded;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject is FrameworkElement)
                ((FrameworkElement)AssociatedObject).Loaded -= XDataTriggerLoaded;

        }

        void XDataTriggerLoaded(object sender, RoutedEventArgs e)
        {
           //EvaluateBindingChange(null);
        }
    }
}

#endif
