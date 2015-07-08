using System.Windows;

namespace LogoFX.Client.Core
{
    /// <summary>
    /// This is indended  to deal with missed visual and logical tree inheritance on some of badly designed controls
    /// </summary>
    public class CommonProperties
    {
        public static DependencyObject GetOwner(DependencyObject obj)
        {
            return (DependencyObject)obj.GetValue(OwnerProperty);
        }

        public static void SetOwner(DependencyObject obj, DependencyObject value)
        {
            obj.SetValue(OwnerProperty, value);
        }

        // Using a DependencyProperty as the backing store for Owner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OwnerProperty =
            DependencyProperty.RegisterAttached("Owner", typeof(DependencyObject), typeof(CommonProperties), new PropertyMetadata(null));
    }
}
