using System;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    public interface INavigationService
    {
        IRootableNavigationBuilder<T> RegisterViewModel<T>(IIocContainer container) where T : class;

        IRootableNavigationBuilder<T> RegisterViewModel<T>(T viewModel);

        IRootableNavigationBuilder<T> RegisterViewModel<T>(Func<T> createFunc);

        void Navigate<T>(object argument = null);        
        void Navigate(Type itemType, object argument = null);

        NavigationParameter CreateParameter<T>(object argument/*, bool noTrack = false*/);
        NavigationParameter CreateParameter<T>();

        void ClearHistory(bool clearSingletones);

        void Back();
        void Forward();

        bool CanNavigateBack { get; }
        bool CanNavigateForward { get; }

        object Current { get; }
    }
}