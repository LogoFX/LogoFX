using System;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    internal sealed partial class NavigationService : INavigationService
    {
        IRootableNavigationBuilder<T> INavigationService.RegisterViewModel<T>(IIocContainer container)
        {
            var builder = new GenericBuilder<T>(container);
            _builders.Add(typeof(T), builder);
            return builder;
        }

        IRootableNavigationBuilder<T> INavigationService.RegisterViewModel<T>(T viewModel)
        {
            var builder = new InstanceBuilder<T>(this, viewModel);
            _builders[viewModel.GetType()] = builder;
            return builder;
        }

        IRootableNavigationBuilder<T> INavigationService.RegisterViewModel<T>(Func<T> createFunc)
        {
            var builder = new ResolverBuilder<T>(this, createFunc);
            _builders.Add(typeof(T), builder);
            return builder;
        }

        async void INavigationService.Navigate<T>(object argument = null)
        {
            await NavigateInternal(typeof(T), argument);
        }

        async void INavigationService.Navigate(Type itemType, object argument = null)
        {
            await NavigateInternal(itemType, argument);
        }

        //public async void NavigateNoTrack<T>(object argument = null)
        //{
        //    StopTrack = true;
        //    try
        //    {
        //        await NavigateInternal(typeof(T), argument);
        //    }
        //    finally
        //    {
        //        StopTrack = false;
        //    }
        //}

        NavigationParameter INavigationService.CreateParameter<T>(object argument/*, bool noTrack = false*/)
        {
            return CreateParameter<T>(argument);
        }

        NavigationParameter INavigationService.CreateParameter<T>()
        {
            return CreateParameter<T>(null);
        }

        private NavigationParameter CreateParameter<T>(object argument)
        {
            return new NavigationParameter<T>(this/*, noTrack*/, argument);
        }

        void INavigationService.ClearHistory(bool clearSingletons)
        {
            _currentIndex = -1;
            _history.Clear();
            if (clearSingletons)
            {
                foreach (var navigationBuilder in _builders.Values)
                {
                    var builder = (NavigationBuilder) navigationBuilder;
                    if (builder.IsRoot == false)
                    {
                        builder.ClearSingleton();
                    }
                }
            }
            UpdateProperties();
        }

        public async void Back()
        {
            HistoryItem historyItem = _history[_currentIndex];
            var navigationModel = historyItem.Object.Target as IAsyncNavigationViewModel;
            if (navigationModel != null)
            {
                bool canNavigate = await navigationModel.BeforeNavigationOutAsync(NavigationDirection.Backward);
                if (!canNavigate)
                {
                    return;
                }
            }

            do
            {
                --_currentIndex;
                historyItem = _history[_currentIndex];
            } while (historyItem.Skip);
            UpdateProperties();

            StopTrack = true;
            try
            {
                await NavigateInternal(historyItem.Type, historyItem.Argument, true);
            }

            finally
            {
                StopTrack = false;
            }
        }

        public async void Forward()
        {
            HistoryItem historyItem = _history[_currentIndex];
            var navigationModel = historyItem.Object.Target as IAsyncNavigationViewModel;
            if (navigationModel != null)
            {
                bool canNavigate = await navigationModel.BeforeNavigationOutAsync(NavigationDirection.Forward);
                if (!canNavigate)
                {
                    return;
                }
            }

            do
            {
                ++_currentIndex;
                historyItem = _history[_currentIndex];
            } while (historyItem.Skip);

            UpdateProperties();
            StopTrack = true;
            try
            {
                await NavigateInternal(historyItem.Type, historyItem.Argument, true);
            }
            finally
            {
                StopTrack = false;
            }
        }

        public bool CanNavigateBack
        {
            get
            {
                int index = _currentIndex - 1;
                while (index > 0 && _history[index].Skip)
                {
                    --index;
                }
                return index >= 0;
            }
        }

        public bool CanNavigateForward
        {
            get
            {
                int index = _currentIndex + 1;
                while (index < _history.Count && _history[index].Skip)
                {
                    ++index;
                }
                return index < _history.Count;
            }
        }

        public object Current
        {
            get
            {
                if (_currentIndex >= _history.Count || _currentIndex < 0)
                {
                    return null;
                }

                return _history[_currentIndex].Object.Target;
            }
        }
    }
}