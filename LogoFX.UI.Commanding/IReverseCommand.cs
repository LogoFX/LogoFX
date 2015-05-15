// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;
using System.Windows.Input;

namespace LogoFX.UI.Commanding
{
    /// <summary>
    /// Represents an <see cref="ICommand">ICommand</see> who's execution can be handled in the View.
    /// </summary>
    public interface IReverseCommand
		 : ICommand
    {
        /// <summary>
        /// Occurs when the <see cref="ICommand">ICommand</see> is executed.
        /// </summary>
        event EventHandler<CommandEventArgs> CommandExecuted;
    }
}
