// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;
using System.Windows.Input;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Extended command with additional attributes
    /// </summary>
    public interface IExtendedCommand:ICommand
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Description { get; set; }

        /// <summary>
        /// Gets the image URI.
        /// </summary>
        Uri ImageUri { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is advanced.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is advanced; otherwise, <c>false</c>.
        /// </value>
        bool IsAdvanced { get; set; }
    }
}
