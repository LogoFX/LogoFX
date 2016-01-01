using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LogoFX.Client.Mvvm.View.Infra.Controls
{
    [TemplatePart(Name = "Popup",Type = typeof(Popup))]
    [TemplatePart(Name = "ClickHandler", Type = typeof(FrameworkElement))]
    public class PopupHeaderedContentControl : HeaderedContentControl, IUpdateVisualState
    {
        private PopupHelper InternalPopup = null;


        public event RoutedPropertyChangingEventHandler<bool> DropDownOpening;

 
        public event RoutedPropertyChangedEventHandler<bool> DropDownOpened;


        public event RoutedPropertyChangingEventHandler<bool> DropDownClosing;

 
        public event RoutedPropertyChangedEventHandler<bool> DropDownClosed;

#if !SILVERLIGHT
        static PopupHeaderedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupHeaderedContentControl), new FrameworkPropertyMetadata(typeof(PopupHeaderedContentControl)));
        }
#endif
        public PopupHeaderedContentControl()
        {
#if SILVERLIGHT
            this.DefaultStyleKey = typeof(PopupHeaderedContentControl);
#endif
            Interaction = new InteractionHelper(this);
            
        }

        #region PopupHorizontalOffset dependency property

        public double PopupHorizontalOffset
        {
            get { return (double)GetValue(PopupHorizontalOffsetProperty); }
            set { SetValue(PopupHorizontalOffsetProperty, value); }
        }

        public static readonly DependencyProperty PopupHorizontalOffsetProperty =
            DependencyProperty.Register("PopupHorizontalOffset", typeof (double), typeof (PopupHeaderedContentControl), new PropertyMetadata(default(double), OnPopupOffsetChanged));

        private static void OnPopupOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #endregion

        #region PopupVerticalOffset dependency property

        public double PopupVerticalOffset
        {
            get { return (double)GetValue(PopupVerticalOffsetProperty); }
            set { SetValue(PopupVerticalOffsetProperty, value); }
        }

        public static readonly DependencyProperty PopupVerticalOffsetProperty =
            DependencyProperty.Register("PopupVerticalOffset", typeof (double), typeof (PopupHeaderedContentControl), new PropertyMetadata(default(double), OnPopupVerticalOffsetChanged));

        private static void OnPopupVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #endregion

        #region PopupPlacement dependency property

        public PopupPlacement PopupPlacement
        {
            get { return (PopupPlacement)GetValue(PopupPlacementProperty); }
            set { SetValue(PopupPlacementProperty, value); }
        }

        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.Register("PopupPlacement", typeof (PopupPlacement), typeof (PopupHeaderedContentControl), new PropertyMetadata(default(PopupPlacement), OnPopupPlacementChanged));

        private static void OnPopupPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #endregion

        #region IsPopupOpen dependency property

        public bool IsPopupOpen
        {
            get { return (bool)GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }

        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register("IsPopupOpen", typeof (bool), typeof (PopupHeaderedContentControl), new PropertyMetadata(default(bool), OnIsPopupOpenChanged));

        private bool _popupHasOpened;
        private bool _ignorePropertyChange;
        private InteractionHelper Interaction;
        private FrameworkElement ClickHandler;

        private static void OnIsPopupOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PopupHeaderedContentControl source = d as PopupHeaderedContentControl;

            // Ignore the change if requested
            if (source._ignorePropertyChange)
            {
                source._ignorePropertyChange = false;
                return;
            }

            bool oldValue = (bool)e.OldValue;
            bool newValue = (bool)e.NewValue;

            if (newValue)
            {
                source.OpeningDropDown(false);
                if (source.InternalPopup != null)
                {
                    source.InternalPopup.Arrange();
                }
            }
            else
            {
                source.ClosingDropDown(oldValue);
            }

            source.UpdateVisualState(true);            
        }

        #endregion

        public override void OnApplyTemplate()
        {
            if (InternalPopup != null)
            {
                InternalPopup.Closed -= DropDownPopup_Closed;
               
                InternalPopup.FocusChanged -= OnDropDownFocusChanged;
                InternalPopup.UpdateVisualStates -= OnDropDownPopupUpdateVisualStates;
                InternalPopup.BeforeOnApplyTemplate();
                InternalPopup = null;
            }

            base.OnApplyTemplate();

            // Set the template parts. Individual part setters remove and add 
            // any event handlers.
            Popup popup = GetTemplateChild("Popup") as Popup;
            if (popup != null)
            {
                InternalPopup = new PopupHelper(this, popup);
                //todo
                InternalPopup.MaxDropDownHeight = 300;
                InternalPopup.AfterOnApplyTemplate();
                InternalPopup.Closed += DropDownPopup_Closed;
                InternalPopup.FocusChanged += OnDropDownFocusChanged;
                InternalPopup.UpdateVisualStates += OnDropDownPopupUpdateVisualStates;

            }

            ClickHandler = GetTemplateChild("ClickHandler") as FrameworkElement;
            if (ClickHandler != null) 
                ClickHandler.MouseLeftButtonDown += ClickHandler_MouseLeftButtonDown;

            Interaction.OnApplyTemplateBase();

            // If the drop down property indicates that the popup is open,
            // flip its value to invoke the changed handler.
            if (IsPopupOpen && InternalPopup != null && !InternalPopup.IsOpen)
            {
                OpeningDropDown(false);
            }
        }


        private void OnDropDownFocusChanged(object sender, EventArgs e)
        {
           
            if (IsPopupOpen)
                ClosingDropDown(true); 

        }


        void ClickHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!IsPopupOpen)
                OpeningDropDown(false);
            else
                ClosingDropDown(true);
            e.Handled = false;
        }

        private void OpeningDropDown(bool oldValue)
        {
            RoutedPropertyChangingEventArgs<bool> args = new RoutedPropertyChangingEventArgs<bool>(IsPopupOpenProperty, oldValue, true, true);

            // Opening
            OnDropDownOpening(args);

            if (args.Cancel)
            {
                _ignorePropertyChange = true;
                SetValue(IsPopupOpenProperty, oldValue);
            }
            else
            {
                //RaiseExpandCollapseAutomationEvent(oldValue, true);
                OpenDropDown(oldValue, true);
            }

            UpdateVisualState(true);
        }

        private void OpenDropDown(bool oldValue, bool newValue)
        {
            IsPopupOpen = true;
            if (InternalPopup != null)
            {
                InternalPopup.IsOpen = true;
            }
            _popupHasOpened = true;
            OnDropDownOpened(new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue));
        }


        protected virtual void OnDropDownOpening(RoutedPropertyChangingEventArgs<bool> e)
        {
            RoutedPropertyChangingEventHandler<bool> handler = DropDownOpening;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDropDownOpened(RoutedPropertyChangedEventArgs<bool> e)
        {
            RoutedPropertyChangedEventHandler<bool> handler = DropDownOpened;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDropDownClosing(RoutedPropertyChangingEventArgs<bool> e)
        {
            RoutedPropertyChangingEventHandler<bool> handler = DropDownClosing;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDropDownClosed(RoutedPropertyChangedEventArgs<bool> e)
        {
            RoutedPropertyChangedEventHandler<bool> handler = DropDownClosed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void DropDownPopup_Closed(object sender, EventArgs e)
        {
            // Force the drop down dependency property to be false.
            if (IsPopupOpen)
            {
                IsPopupOpen = false;
            }

            // Fire the DropDownClosed event
            if (_popupHasOpened)
            {
                OnDropDownClosed(new RoutedPropertyChangedEventArgs<bool>(true, false));
            }
        }


        internal virtual void UpdateVisualState(bool useTransitions)
        {
            // Popup
            VisualStateManager.GoToState(this, IsPopupOpen ? VisualStates.StatePopupOpened : VisualStates.StatePopupClosed, useTransitions);

            // Handle the Common and Focused states
            Interaction.UpdateVisualStateBase(useTransitions);
        }

        private void OnDropDownPopupUpdateVisualStates(object sender, EventArgs e)
        {
            UpdateVisualState(true);
        }

        private void ClosingDropDown(bool oldValue)
        {
            bool delayedClosingVisual = false;
            if (InternalPopup != null)
            {
                delayedClosingVisual = InternalPopup.UsesClosingVisualState;
            }

            RoutedPropertyChangingEventArgs<bool> args = new RoutedPropertyChangingEventArgs<bool>(IsPopupOpenProperty, oldValue, false, true);

            OnDropDownClosing(args);

            

            if (args.Cancel)
            {
                _ignorePropertyChange = true;
                SetValue(IsPopupOpenProperty, oldValue);
            }
            else
            {
                // Immediately close the drop down window:
                // When a popup closed visual state is present, the code path is 
                // slightly different and the actual call to CloseDropDown will 
                // be called only after the visual state's transition is done
                //RaiseExpandCollapseAutomationEvent(oldValue, false);
                if (!delayedClosingVisual)
                {
                    CloseDropDown(oldValue, false);
                }
            }

            UpdateVisualState(true);
        }


        private void CloseDropDown(bool oldValue, bool newValue)
        {
            if (_popupHasOpened)
            {
                if (InternalPopup != null)
                {
                    InternalPopup.IsOpen = false;
                }
                OnDropDownClosed(new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue));
            }
        }

        void IUpdateVisualState.UpdateVisualState(bool useTransitions)
        {
            UpdateVisualState(useTransitions);
        }

    }
}
