using System;

namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// <see cref="IViewModel"/> that wraps some object
    /// </summary>
    public interface IObjectViewModel : IViewModel, IDisposable, IModelWrapper
    {
    }

    /// <summary>
    /// <see cref="IViewModel"/> that wraps some object
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    public interface IObjectViewModel<out T> : IObjectViewModel, IModelWrapper<T>
    {
        /// <summary>
        /// Gets the object model.
        /// </summary>
        /// <value>The object model.</value>
        [Obsolete("Use IMomelWrapper<T>.Model")]
         T ObjectModel { get;}
    }
}