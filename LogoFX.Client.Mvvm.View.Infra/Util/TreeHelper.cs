using System.Collections.Generic;
using System.Linq;
using System.Windows;
#if !WinRT
using System.Windows.Media;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Util
{
    public static class TreeHelper
    {
        public static T GetVisualDescendant<T>(this DependencyObject d)
        {
            return GetVisualDescendants<T>(d).FirstOrDefault();
        }

        public static IEnumerable<T> GetVisualDescendants<T>(this DependencyObject d)
        {
            for (int n = 0; n < VisualTreeHelper.GetChildrenCount(d); n++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(d, n);

                if (child is T)
                {
                    yield return (T)(object)child;
                }

                foreach (T match in GetVisualDescendants<T>(child))
                {
                    yield return match;
                }
            }
        }

        public static T FindVisualAncestor<T>(this DependencyObject obj, bool includeThis) where T : DependencyObject
        {
            if (!includeThis)
                obj = VisualTreeHelper.GetParent(obj);

            while (obj != null && (!(obj is T)))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            return obj as T;
        }
#if !SILVERLIGHT && !WinRT
        public static T FindLogicalAncestor<T>(this DependencyObject obj, bool includeThis) where T : DependencyObject
        {
            if (!includeThis)
                obj = LogicalTreeHelper.GetParent(obj);

            while (obj != null && (!(obj is T)))
            {
                obj = LogicalTreeHelper.GetParent(obj);
            }
            return obj as T;
        }
#endif
    }
}
