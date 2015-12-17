using System;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation service which allows navigation within the application
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Registers the view model for navigation using container-resolution strategy
        /// </summary>
        /// <typeparam name="T">Type of view model</typeparam>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        IRootableNavigationBuilder<T> RegisterViewModel<T>(IIocContainer container) where T : class;

        IRootableNavigationBuilder<T> RegisterViewModel<T>(T viewModel);

        IRootableNavigationBuilder<T> RegisterViewModel<T>(Func<T> createFunc);

        void Navigate<T>(object argument = null);        
        void Navigate(Type itemType, object argument = null);

        NavigationParameter CreateParameter<T>(object argument/*, bool noTrack = false*/);
        NavigationParameter CreateParameter<T>();

        void ClearHistory(bool clearSingletons);

        void Back();
        void Forward();

        bool CanNavigateBack { get; }
        bool CanNavigateForward { get; }

        object Current { get; }
    }
}