using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace LogoFX.Client.Mvvm.View.Infra.Interactivity.Actions
{
    [ContentProperty("Parameter")]
    public class ExecuteCommandAction
#if WP7
        : BindableTriggerAction<FrameworkElement>
#else
 : TriggerAction<UIElement>
#endif
    {
        private const double INTERACTIVITY_ENABLED = 1d;
        private const double INTERACTIVITY_DISABLED = 0.5d;

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ExecuteCommandAction),
            new PropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));

        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(object), typeof(ExecuteCommandAction),
            new PropertyMetadata(null, new PropertyChangedCallback(OnParameterChanged)));

        public static readonly DependencyProperty TriggerParameterConverterProperty =
            DependencyProperty.Register("TriggerParameterConverter", typeof(IValueConverter), typeof(ExecuteCommandAction),
            new PropertyMetadata(null));

        public bool UseTriggerParameter
        {
            get { return (bool)GetValue(UseTriggerParameterProperty); }
            set { SetValue(UseTriggerParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UseTriggerParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseTriggerParameterProperty =
            DependencyProperty.Register("UseTriggerParameter", typeof(bool), typeof(ExecuteCommandAction), new PropertyMetadata(false));


        private bool _manageEnableState = true;

        #region Properties


#if (!WP7)
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


#if WP7
        [TypeConverter(typeof(nRoute.Components.TypeConverters.ConvertFromStringConverter))]
#endif
#if (!WP7)
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public object Parameter
        {
            get { return GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }


#if (!WP7)
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public IValueConverter TriggerParameterConverter
        {
            get { return (IValueConverter)GetValue(TriggerParameterConverterProperty); }
            set { SetValue(TriggerParameterConverterProperty, value); }
        }


        public bool ManageEnableState
        {
            get { return _manageEnableState; }
            set { _manageEnableState = value; }
        }

#if WP7

        
#if (!WP7)
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public Binding CommandBinding
        {
            get { return GetBinding(CommandProperty); }
            set { SetBinding<ICommand>(CommandProperty, value); }
        }

        
#if (!WP7)
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public Binding ParameterBinding
        {
            get { return GetBinding(ParameterProperty); }
            set { SetBinding<object>(ParameterProperty, value); }
        }

        
#if (!WP7)
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public Binding TriggerParameterConverterBinding
        {
            get { return GetBinding(TriggerParameterConverterProperty); }
            set { SetBinding<IValueConverter>(TriggerParameterConverterProperty, value); }
        }

#endif

        #endregion

        #region Trigger Related

        protected override void OnAttached()
        {
            base.OnAttached();
            UpdateEnabledState();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            DisposeEnableState();
        }

        protected override void Invoke(object arg)
        {
            if (this.AssociatedObject == null) return;

            // if a trigger parameter converter is specified, then we use that to get the command parameter
            // else we use the given parameter - note_ the parameter can be null
            var parameter = TriggerParameterConverter != null ?
                TriggerParameterConverter.Convert(arg, typeof(Object), this.AssociatedObject, CultureInfo.CurrentCulture) :
                this.Parameter;
            if (parameter == null && UseTriggerParameter)
                parameter = arg;

            if (this.Command != null && this.Command.CanExecute(parameter))
                this.Command.Execute(parameter);
        }

        #endregion

        #region Handlers

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ExecuteCommandAction)d).SetupEnableState(e.NewValue as ICommand, e.OldValue as ICommand);
        }

        private static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ExecuteCommandAction)d).UpdateEnabledState();
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            this.UpdateEnabledState();
        }

        #endregion

        #region Helpers

        private void SetupEnableState(ICommand newCommand, ICommand oldCommand)
        {
            if (!ManageEnableState) return;

            // we detach or attach
            if (oldCommand != null)
                oldCommand.CanExecuteChanged -= new EventHandler(Command_CanExecuteChanged);
            if (newCommand != null)
                newCommand.CanExecuteChanged += new EventHandler(Command_CanExecuteChanged);

            // and update
            UpdateEnabledState();
        }

        private void UpdateEnabledState()
        {
            if (!ManageEnableState || AssociatedObject == null || Command == null) return;

            // we get if it is enabled or not
            var _canExecute = this.Command.CanExecute(this.Parameter);

            // we check if it is a control in SL
#if (SILVERLIGHT)          
            if (typeof(Control).IsAssignableFrom(AssociatedObject.GetType()))
            {
                var _target = AssociatedObject as Control;
                _target.IsEnabled = _canExecute;
            }
            else
            {
                AssociatedObject.IsHitTestVisible = _canExecute;
                AssociatedObject.Opacity = _canExecute ? INTERACTIVITY_ENABLED : INTERACTIVITY_DISABLED;
            }
#else
            AssociatedObject.IsEnabled = _canExecute;
#endif
        }

        private void DisposeEnableState()
        {
            if (!ManageEnableState || AssociatedObject == null || Command == null) return;

#if (SILVERLIGHT)          
            if (AssociatedObject as Control != null)
#else
            if (AssociatedObject != null)
#endif
            {
                Command.CanExecuteChanged -= new EventHandler(Command_CanExecuteChanged);
            }
        }

        #endregion

    }
}
