using System.Windows;
using System.Windows.Controls;

namespace LogoFX.Client.Mvvm.View.Infra.Controls
{
    public class HeaderedContentControl : ContentControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(HeaderedContentControl), new PropertyMetadata(OnHeaderPropertyChanged));
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HeaderedContentControl), new PropertyMetadata(OnHeaderTemplatePropertyChanged));

        public object Header
        {
            get
            {
                return this.GetValue(HeaderProperty);
            }
            set
            {
                this.SetValue(HeaderProperty, value);
            }
        }

        public DataTemplate HeaderTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(HeaderTemplateProperty);
            }
            set
            {
                this.SetValue(HeaderTemplateProperty, value);
            }
        }

        static HeaderedContentControl()
        {
        }

        public HeaderedContentControl()
        {
            this.DefaultStyleKey = typeof(HeaderedContentControl);
        }

        private static void OnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HeaderedContentControl)d).OnHeaderChanged(e.OldValue, e.NewValue);
        }

        private static void OnHeaderTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HeaderedContentControl)d).OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }

        protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
        {
        }

        protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
        {
        }
    }
}
