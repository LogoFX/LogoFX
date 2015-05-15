using System.Windows;
using System.Windows.Interactivity;

namespace LogoFX.UI.Views.Infra.Interactivity.Actions
{
    [TypeConstraintAttribute(typeof(DependencyObject))]
    public class TargetedSetFocusAction
#if SILVERLIGHT || WinRT
        : TargetedTriggerAction<Control>
#else
 : TargetedTriggerAction<UIElement>
#endif
    {
        protected override void Invoke(object parameter)
        {
            if (Target != null)
            {
#if WinRT
                bool focused = Target.Focus(FocusState.Programmatic);
#else
                bool focused = Target.Focus();
#endif
            }
        }
    }
}
