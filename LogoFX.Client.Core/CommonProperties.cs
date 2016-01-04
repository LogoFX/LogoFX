using System.Windows;

namespace LogoFX.Client.Core
{
    /// <summary>
    /// This is indended  to deal with missed visual and logical tree inheritance on some of poorly designed controls
    /// </summary>
    public class CommonProperties
    {
        /// <summary>
        /// Gets the owner value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DependencyObject GetOwner(DependencyObject obj)
        {
            return (DependencyObject)obj.GetValue(OwnerProperty);
        }

        /// <summary>
        /// Sets the owner value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetOwner(DependencyObject obj, DependencyObject value)
        {
            obj.SetValue(OwnerProperty, value);
        }

        // Using a DependencyProperty as the backing store for Owner.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Owner which is usually the parent.
        /// </summary>
        public static readonly DependencyProperty OwnerProperty =
            DependencyProperty.RegisterAttached("Owner", typeof(DependencyObject), typeof(CommonProperties), new PropertyMetadata(null));
    }
}
