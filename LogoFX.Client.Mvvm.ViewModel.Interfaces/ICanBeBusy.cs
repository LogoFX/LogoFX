// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces
{
    /// <summary>
    /// Designates object with busy state.
    /// </summary>
    public interface ICanBeBusy
    {
        /// <summary>
        /// Gets or sets a value indicating whether the object is busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the object is busy; otherwise, <c>false</c>.
        /// </value>
        bool IsBusy { get; set; }
    }
}
