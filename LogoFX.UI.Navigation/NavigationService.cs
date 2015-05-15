using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace LogoFX.UI.Navigation
{
    internal sealed partial class NavigationService : PropertyChangedBase
    {
        #region Nested Types

        /// <summary>
        /// The History Item.
        /// </summary>
        private sealed class HistoryItem
        {
            /// <summary>
            /// Type of this item.
            /// </summary>
            public Type Type { get; private set; }

            public object Argument { get; private set; }

            public WeakReference Object { get; set; }

            public bool Skip { get; private set; }

            public HistoryItem(Type type, object argument, bool skip)
            {
                Type = type;
                Argument = argument;
                Skip = skip;
            }
        }

        #endregion

        #region Fields

        private int _stopTrack;
        private int _stopEvents;

        private int _currentIndex = -1;

        private readonly List<HistoryItem> _history =
            new List<HistoryItem>();

        private readonly Dictionary<Type, INavigationBuilder> _builders = 
            new Dictionary<Type, INavigationBuilder>();

        #endregion

        #region Internal Members

        internal void RegisterAttr(Type type, NavigationViewModelAttribute attr)
        {
            var types = new List<Type> {type};
            var synonymAttrs = type.GetAttributes<NavigationSynonymAttribute>(false);
            types.AddRange(synonymAttrs.Select(x => x.SynonimType));

            foreach (var t in types)
            {
                var builder = new AttributeBuilder(type, attr);
                _builders.Add(t, builder);
            }
        }

        #endregion

        #region Private Members

        private bool StopTrack
        {
            get { return _stopTrack > 0; }
            set
            {
                if (value)
                {
                    ++_stopTrack;
                }
                else
                {
                    --_stopTrack;
                }

                Debug.Assert(_stopTrack >= 0);
            }
        }

        private bool StopEvents
        {
            get { return _stopEvents > 0; }
            set
            {
                if (value)
                {
                    ++_stopEvents;
                }
                else
                {
                    --_stopEvents;
                }

                Debug.Assert(_stopEvents >= 0);
            }
        }

        private void UpdateProperties()
        {
            NotifyOfPropertyChange(() => CanNavigateBack);
            NotifyOfPropertyChange(() => CanNavigateForward);
            NotifyOfPropertyChange(() => Current);
        }

        private INavigationBuilder GetBuilder(Type type)
        {
            INavigationBuilder navigationBuilder;

            if (!_builders.TryGetValue(type, out navigationBuilder))
            {
                throw new SystemException(String.Format("Not registered type '{0}'.", type));
            }

            return navigationBuilder;
        }

        private async Task<INavigationConductor> ActivateConductorAsync(Type conductorType)
        {
            var builder = GetBuilder(conductorType);

            if (builder.IsRoot)
            {
                return (INavigationConductor) builder.GetValue();
            }

            INavigationConductor result;

            StopTrack = true;
            StopEvents = true;
            try
            {
                result = (INavigationConductor) await NavigateInternal(conductorType, null, true);
            }

            finally
            {
                StopEvents = false;
                StopTrack = false;
            }

            return result;
        }        

        private async Task<object> NavigateInternal(Type itemType, object argument, bool noCheckHistory = false)
        {
            if (_currentIndex >= 0 && _history.Count > 0 && !noCheckHistory)
            {
                //if current is same v-m
                int index = _currentIndex;
                while (index >= 0 && _history[index].Skip)
                {
                    --index;
                }
                HistoryItem historyItem = _history[index];
                var obj = historyItem.Object.Target;
                if (historyItem.Type == itemType && historyItem.Argument == argument && obj != null)
                {
                    if (index != _currentIndex)
                    {
                        var obj2 = obj as INavigationViewModel;
                        if (!StopEvents && obj2 != null)
                        {
                            obj2.OnNavigated(NavigationDirection.None, argument);
                        }
                        _currentIndex = index;
                        UpdateProperties();
                    }
                    return obj;
                }

                //work with current item
                historyItem = _history[_currentIndex];
                obj = historyItem.Object.Target;
                var navigationModel = obj as IAsyncNavigationViewModel;
                if (navigationModel != null)
                {
                    bool canNavigate;
                    try
                    {
                        canNavigate = await navigationModel.BeforeNavigationOutAsync(NavigationDirection.None);
                    }
                    catch (Exception err)
                    {
                        Trace.TraceError("BeforeNavigationOutAsync throws error: {0}", err);
                        throw;
                    }
                    if (!canNavigate)
                    {
                        return obj;
                    }
                }
            }

            var builder = GetBuilder(itemType);            
            INavigationConductor conductor;
            try
            {
                conductor = await ActivateConductorAsync(builder.ConductorType);
            }
            catch (Exception err)
            {
                Trace.TraceError("ActivateConductorAsync throws error: {0}", err);                
                throw;
            }
            object viewModel = builder.GetValue();
            conductor.NavigateTo(viewModel, argument);

            var navigationViewModel = viewModel as INavigationViewModel;
            if (!StopEvents && navigationViewModel != null)
            {
                navigationViewModel.OnNavigated(NavigationDirection.None, argument);
            }
            
            if (!StopTrack)
            {
                ++_currentIndex;
                Debug.Assert(_history.Count >= _currentIndex);
                if (_history.Count != _currentIndex)
                {
                    _history.RemoveRange(_currentIndex, _history.Count - _currentIndex);
                }

                var historyItem = new HistoryItem(itemType, argument, builder.NotRemember)
                {
                    Object = new WeakReference(viewModel)
                };
                _history.Insert(_currentIndex, historyItem);
                UpdateProperties();
            }

            return viewModel;
        }

        #endregion
    }
}