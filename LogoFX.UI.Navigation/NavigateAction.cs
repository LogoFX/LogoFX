using System;
using System.Windows;
using System.Windows.Interactivity;

namespace LogoFX.UI.Navigation
{
    public sealed class NavigateAction : TriggerAction<UIElement>
    {
        #region Fields

        private static readonly Type s_thisType = typeof(NavigateAction);

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(
                "Parameter",
                typeof(NavigationParameter),
                s_thisType);

        public static readonly DependencyProperty ItemTypeProperty =
            DependencyProperty.Register(
                "ItemType",
                typeof(Type),
                s_thisType);

        public static readonly DependencyProperty ArgumentProperty =
            DependencyProperty.Register(
                "Argument",
                typeof(object),
                s_thisType);

        public static readonly DependencyProperty NavigationServiceProperty =
            DependencyProperty.Register(
                "NavigationService",
                typeof (INavigationService),
                s_thisType);

        #endregion

        #region Public Properties

        public NavigationParameter Parameter
        {
            get { return (NavigationParameter)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        public Type ItemType
        {
            get { return (Type)GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }

        public object Argument
        {
            get { return GetValue(ArgumentProperty); }
            set { SetValue(ArgumentProperty, value); }
        }

        public INavigationService NavigationService
        {
            get { return (INavigationService)GetValue(NavigationServiceProperty); }
            set { SetValue(NavigationServiceProperty, value); }
        }

        #endregion

        protected override void Invoke(object parameter)
        {          
            if (NavigationService == null)
            {
                throw new Exception("Navigation service must be set");
            }

            if (Parameter != null)
            {
                Parameter.Navigate();
            }
            else if (ItemType != null)
            {
                NavigationService.Navigate(ItemType, Argument);
            }
            else
            {
                throw new SystemException("You must set Parameter or ItemType.");
            }
        }
    }
}