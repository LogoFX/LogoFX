using System;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Contains information about models collection
    /// </summary>
    public interface IInfoModelsCollection
    {
        /// <summary>
        /// Overall items count
        /// </summary>
        int ItemsCount { get; }

        /// <summary>
        /// True if collection contains items, false otherwise
        /// </summary>
        bool HasItems { get; }

        /// <summary>
        /// Raised when <see cref="HasItems"/> is changed
        /// </summary>
        event EventHandler HasItemsChanged;
    }
}