#if !WinRT
using System.Windows;
using System.Windows.Interactivity;
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

namespace LogoFX.Client.Mvvm.View.Infra.Interactivity.Actions
{
    public class SetFocusAction
#if SILVERLIGHT
        : TriggerAction<System.Windows.Controls.Control>
#elif !WinRT
 : TriggerAction<UIElement>
#else
 : TriggerAction<Control>
#endif
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject != null) AssociatedObject.Focus
                (
#if WinRT
                FocusState.Programmatic
#endif
                );
        }
    }
}