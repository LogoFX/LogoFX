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

        /// <summary>
        /// Registers the view model instance for navigation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        IRootableNavigationBuilder<T> RegisterViewModel<T>(T viewModel);

        /// <summary>
        /// Registers the view model creator function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="createFunc">The create function.</param>
        /// <returns></returns>
        IRootableNavigationBuilder<T> RegisterViewModel<T>(Func<T> createFunc);

        /// <summary>
        /// Navigates to the specified target using the specified argument.
        /// </summary>
        /// <typeparam name="T">Type of the navigation target.</typeparam>
        /// <param name="argument">The argument.</param>
        void Navigate<T>(object argument = null);

        /// <summary>
        /// Navigates to the specified target using the specified argument.
        /// </summary>
        /// <param name="itemType">Type of the navigation target.</param>
        /// <param name="argument">The argument.</param>
        void Navigate(Type itemType, object argument = null);

        /// <summary>
        /// Creates navigation parameter using navigation target and specified argument.
        /// </summary>
        /// <param name="argument"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        NavigationParameter CreateParameter<T>(object argument/*, bool noTrack = false*/);

        /// <summary>
        /// Creates navigation parameter using navigation target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        NavigationParameter CreateParameter<T>();

        /// <summary>
        /// Clears navigation history.
        /// </summary>
        /// <param name="clearSingletons"></param>
        void ClearHistory(bool clearSingletons);

        /// <summary>
        /// Navigates back.
        /// </summary>
        void Back();

        /// <summary>
        /// Navigates forward.
        /// </summary>
        void Forward();

        /// <summary>
        /// Returns value indicating whether can navigate back.
        /// </summary>
        /// <value>
        /// <c>true</c> if can navigate back; otherwise, <c>false</c>.
        /// </value>
        bool CanNavigateBack { get; }

        /// <summary>
        /// Gets a value indicating whether can navigate forward.
        /// </summary>
        /// <value>
        /// <c>true</c> if can navigate forward; otherwise, <c>false</c>.
        /// </value>
        bool CanNavigateForward { get; }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        /// <value>
        /// The current item.
        /// </value>
        object Current { get; }
    }
}