#region Copyright
// Partial Copyright (c) LogoUI Software Solutions LTD
// Author: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.
#endregion

using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using LogoFX.Client.Core;
using EventTrigger = System.Windows.Interactivity.EventTrigger;
#if !WinRT

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
    /// <summary>
    /// Provides way to call some named command with notion of visal tree routing
    /// Will search for propety of type <see cref="ICommand"/> with name />
    /// </summary>
#if WinRT
    [ContentProperty(Name="Parameter")]
#else
    [ContentProperty("Parameter")]
    [DefaultTrigger(typeof(FrameworkElement), typeof(EventTrigger), "MouseLeftButtonDown")]
    [DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
#endif
    [TypeConstraint(typeof(FrameworkElement))]
    public class ExecuteNamedCommandAction : TriggerAction<FrameworkElement>
    {
        private const double INTERACTIVITY_ENABLED = 1d;
        private const double INTERACTIVITY_DISABLED = 0.5d;



        public bool UseTriggerParameter
        {
            get { return (bool)GetValue(UseTriggerParameterProperty); }
            set { SetValue(UseTriggerParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UseTriggerParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseTriggerParameterProperty =
            DependencyProperty.Register("UseTriggerParameter", typeof(bool), typeof(ExecuteNamedCommandAction), new PropertyMetadata(false));


        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ExecuteNamedCommandAction), new PropertyMetadata(null, OnCommandChanged));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ExecuteNamedCommandAction executeNamedCommandAction = (ExecuteNamedCommandAction)d;
            if (e.NewValue != null || string.IsNullOrEmpty(executeNamedCommandAction.CommandName))
                executeNamedCommandAction.InternalCommand = e.NewValue as ICommand;
        }

#if (!WP7 && !WinRT)
        [CustomPropertyValueEditor(CustomPropertyValueEditor.PropertyBinding)]
#endif
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Represents the method name of an action message.
        /// </summary>
        public static readonly DependencyProperty CommandNameProperty =
            DependencyProperty.Register(
                "CommandName",
                typeof(string),
                typeof(ExecuteNamedCommandAction),
                null
                );

        /// <summary>
        /// Represents the parameters of an action message.
        /// </summary>
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(
            "Parameter",
            typeof(object),
            typeof(ExecuteNamedCommandAction),
            null
            );

        public static readonly DependencyProperty TriggerParameterConverterProperty =
            DependencyProperty.Register("TriggerParameterConverter", typeof(IValueConverter), typeof(ExecuteNamedCommandAction),
            new PropertyMetadata(null));


        private ICommand internalCommand;
        private ICommand InternalCommand
        {
            get { return internalCommand; }
            set
            {
                if (internalCommand != null)
                    internalCommand.CanExecuteChanged -= CanexecuteChanged;
                internalCommand = value;
                if (internalCommand != null)
                    internalCommand.CanExecuteChanged += CanexecuteChanged;
            }
        }

        public bool ManageEnableState
        {
            get { return _manageEnableState; }
            set { _manageEnableState = value; }
        }

        private void CanexecuteChanged(object sender, EventArgs e)
        {
            UpdateAvailabilityCore();
        }

        /// <summary>
        /// Creates an instance of <see cref="ExecuteNamedCommandAction"/>.
        /// </summary>
        public ExecuteNamedCommandAction()
        {
            //SetValue(ParametersProperty, new AttachedCollection<Parameter>());
        }

        /// <summary>
        /// Gets or sets the name of the command to be invoked on the presentation model class.
        /// </summary>
        /// <value>The name of the method.</value>
        public string CommandName
        {
            get { return (string)GetValue(CommandNameProperty); }
            set { SetValue(CommandNameProperty, value); }
        }

        /// <summary>
        /// Gets the parameters to pass as part of the method invocation.
        /// </summary>
        /// <value>The parameters.</value>
        public object Parameter
        {
            get { return (object)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        public IValueConverter TriggerParameterConverter
        {
            get { return (IValueConverter)GetValue(TriggerParameterConverterProperty); }
            set { SetValue(TriggerParameterConverterProperty, value); }
        }


        /// <summary>
        /// Occurs before the message detaches from the associated object.
        /// </summary>
        public event EventHandler Detaching = delegate { };

        private static bool? isInDesignMode;
        private bool _manageEnableState = true;

        public static bool IsInDesignMode
        {
            get
            {
                if (isInDesignMode == null)
                {
                    var app = Application.Current.ToString();

                    if (app == "System.Windows.Application" || app == "Microsoft.Expression.Blend.BlendApplication")
                        isInDesignMode = true;
                    else isInDesignMode = false;
                }

                return isInDesignMode.GetValueOrDefault(false);
            }
        }
        protected override void OnAttached()
        {
            if (!IsInDesignMode)
            {
                ElementLoaded(null, null);
                AssociatedObject.Loaded += ElementLoaded;
            }

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (!IsInDesignMode)
            {
                Detaching(this, EventArgs.Empty);
                AssociatedObject.Loaded -= ElementLoaded;
                if (internalCommand != null)
                    internalCommand.CanExecuteChanged -= CanexecuteChanged;
            }

            base.OnDetaching();
        }

        void ElementLoaded(object sender, RoutedEventArgs e)
        {
            SetPropertyBinding();
            UpdateAvailabilityCore();
        }

        protected override void Invoke(object arg)
        {


            if (InternalCommand == null)
            {
                SetPropertyBinding();
                if (!UpdateAvailabilityCore())
                    return;
            }

            var parameter = TriggerParameterConverter != null ?
                TriggerParameterConverter.Convert(arg, typeof(Object), this.AssociatedObject, CultureInfo.CurrentCulture
#if WinRT
                .TwoLetterISOLanguageName
#endif
) :
                this.Parameter;
            if (parameter == null && UseTriggerParameter)
                parameter = arg;

            if (this.InternalCommand != null && this.InternalCommand.CanExecute(parameter))
                this.InternalCommand.Execute(parameter);
        }

        /// <summary>
        /// Forces an update of the UI's Enabled/Disabled state based on the the preconditions associated with the method.
        /// </summary>
        public void UpdateAvailability()
        {
            if (AssociatedObject == null)
                return;

            if (InternalCommand == null)
                SetPropertyBinding();

            UpdateAvailabilityCore();
        }

        bool UpdateAvailabilityCore()
        {
            return !ManageEnableState || ApplyAvailabilityEffect();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "NamedCommndAction: " + CommandName;
        }


        /// <summary>
        /// Applies an availability effect, such as IsEnabled, to an element.
        /// </summary>
        /// <remarks>Returns a value indicating whether or not the action is available.</remarks>
        private bool ApplyAvailabilityEffect()
        {

            if (AssociatedObject == null || InternalCommand == null) return false;

            // we get if it is enabled or not
            bool _canExecute = this.InternalCommand.CanExecute(this.Parameter);

            // we check if it is a control in SL
#if SILVERLIGHT || WinRT
            if (AssociatedObject is Control)
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
            return _canExecute;
        }

        /// <summary>
        /// Sets the target, method and view on the context. Uses a bubbling strategy by default.
        /// </summary>
        private void SetPropertyBinding()
        {
            if (string.IsNullOrWhiteSpace(CommandName))
                return;
            DependencyObject currentElement = AssociatedObject;
            PropertyInfo commandProperty = null;
            object currentTarget = null;
            InternalCommand = null;

            while (currentElement != null && InternalCommand == null)
            {
                currentTarget = currentElement.GetValue(FrameworkElement.DataContextProperty);

                if (currentTarget != null)
                    commandProperty = currentTarget.GetType().GetProperty(CommandName);

                DependencyObject temp = null;
                //is have readable property derived from icommand
                if (commandProperty == null || !commandProperty.CanRead || !typeof(ICommand).IsAssignableFrom(commandProperty.PropertyType))
                {

#if !SILVERLIGHT && !WinRT
                    if (currentElement is ContextMenu)
                    {
                        ContextMenu cm = currentElement as ContextMenu;
                        temp = cm.PlacementTarget;
                    }
                    else
#endif
                        temp = VisualTreeHelper.GetParent(currentElement);
                    if (temp == null)
                    {
                        FrameworkElement element = currentElement as FrameworkElement;
                        if (element == null || element.Parent == null)
                        {
                            currentElement = CommonProperties.GetOwner(currentElement) as FrameworkElement;
                        }
                        else
                        {
                            currentElement = element.Parent;
                        }
                    }
                    else
                    {
                        currentElement = temp;
                    }
                }

                else
                {
                    InternalCommand = (ICommand)commandProperty.GetValue(currentTarget, null);
                }
            }

            if (InternalCommand != null)
                return;

            //check associated object itself
            currentTarget = AssociatedObject;

            if (currentTarget != null)
                commandProperty = currentTarget.GetType().GetProperty(CommandName
#if !WinRT
, BindingFlags.FlattenHierarchy
#endif
);

            //is have readable property derived from icommand
            if (commandProperty == null || !commandProperty.CanRead || !typeof(ICommand).IsAssignableFrom(commandProperty.PropertyType))
                return;

            InternalCommand = (ICommand)commandProperty.GetValue(currentTarget, null);
        }
    }
}
