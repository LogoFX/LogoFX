// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;

namespace LogoFX.UI.ViewModels.Interfaces
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