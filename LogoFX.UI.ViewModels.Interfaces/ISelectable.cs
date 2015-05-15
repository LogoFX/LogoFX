// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.UI.ViewModels.Interfaces
{
    /// <summary>
    /// Designates element that can be selected
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Selection status
        /// </summary>
        bool IsSelected { get; set; }

        ///// <summary>
        ///// Selection event
        ///// </summary>
        //event EventHandler Selected;
        ///// <summary>
        ///// Un-selection event
        ///// </summary>
        //event EventHandler UnSelected;
    }
}   