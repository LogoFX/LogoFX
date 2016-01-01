using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
#if WinRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using CultureInfo = System.String;
#else
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace LogoFX.Client.Mvvm.View.Infra.Converters
{
    internal static class ImageGet
    {
        public delegate object MakeRet(string src);
        public static MakeRet GetDelegateOnType(Type targetType)
        {
            MakeRet mk;
            if (targetType == typeof(ImageSource))
            {
                mk = s => new BitmapImage(new Uri(s,UriKind.RelativeOrAbsolute));
            }
            else if (targetType == typeof(Uri))
            {
                mk = s => new Uri(s, UriKind.RelativeOrAbsolute);
            }
            else /*if (targetType == null || targetType == typeof(string) || targetType == typeof(object))*/
            {
                mk = s => s;
            }
            return mk;
        }

    }
    public  class ObjectToImageConverter : IValueConverter
    {
        private string _suffix;
        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; }
        }

        public string Extension
        {
            get { return _ext; }
            set { _ext = value; }
        }
        private string _folder = "Icons";
        private string _ext = ".png";
        private string _default;

        public string Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }

        public string Default
        {
            get { return _default; }
            set { _default = value; }
        }

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageGet.MakeRet mk = ImageGet.GetDelegateOnType(targetType);

            //IOperation c = value as IOperation;
            string folder = Folder;
            if (parameter != null && parameter is string && ((string)parameter).Trim().Length > 0)
                folder = (string) parameter;
            if (value != null)
            {
                try
                {
                    return mk(/*"pack://application:,,,/PG_Client;component/Images/" +*/ folder + "/"
                              + value +
                              (!String.IsNullOrEmpty(Suffix) ? ("_" + Suffix) : "")
                              + Extension);
                }
                catch (Exception)
                {
                    Debug.WriteLine("ImageConverter:" + value + " image is not found");
                }
            }
            return !string.IsNullOrWhiteSpace(Default)?mk(Default): DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
