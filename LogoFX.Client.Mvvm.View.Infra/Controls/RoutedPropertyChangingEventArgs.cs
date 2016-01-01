using System;
using System.Windows;

namespace LogoFX.Client.Mvvm.View.Infra.Controls
{
    public delegate void RoutedPropertyChangingEventHandler<T>(object sender, RoutedPropertyChangingEventArgs<T> e);

    public class RoutedPropertyChangingEventArgs<T> : RoutedEventArgs
    {
        private bool _cancel;

        public DependencyProperty Property { get; private set; }

        public T OldValue { get; private set; }

        public T NewValue { get; set; }

        public bool IsCancelable { get; private set; }

        public bool Cancel
        {
            get
            {
                return this._cancel;
            }
            set
            {
                if (this.IsCancelable)
                    this._cancel = value;
                else if (value)
                    throw new InvalidOperationException("invalid cancel");
            }
        }

        public bool InCoercion { get; set; }

        public RoutedPropertyChangingEventArgs(DependencyProperty property, T oldValue, T newValue, bool isCancelable)
        {
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.IsCancelable = isCancelable;
            this.Cancel = false;
        }

#if !SILVERLIGHT
        public RoutedPropertyChangingEventArgs(DependencyProperty property, T oldValue, T newValue, bool isCancelable, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.IsCancelable = isCancelable;
            this.Cancel = false;
        }
#endif
    }
}
