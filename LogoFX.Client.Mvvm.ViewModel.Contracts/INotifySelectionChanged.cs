using System;

namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Object that notifies about selection change
    /// </summary>
    public interface INotifySelectionChanged
    {
        /// <summary>
        /// Occurs when [selection changed].
        /// </summary>
        event EventHandler SelectionChanged;
        /// <summary>
        /// Occurs when [selection changing].
        /// </summary>
        event EventHandler<SelectionChangingEventArgs> SelectionChanging;
    }
}