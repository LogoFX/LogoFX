// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.UI.ViewModels.Interfaces
{
    /// <summary>
    /// Designates object with busy state
    /// </summary>
    public interface ICanBeBusy
    {
        /// <summary>
        /// Defines busy state
        /// </summary>
        bool IsBusy { get; set; }
    }
}
