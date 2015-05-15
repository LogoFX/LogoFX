// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System.Collections;
using System.Collections.Generic;

namespace LogoFX.UI.ViewModels.Interfaces
{
    /// <summary>
    /// Object that have multiple selected items
    /// </summary>
    /// <typeparam name="T">type of selected items</typeparam>
    public interface 
#if !SILVERLIGHT

        IHaveSelectedItems<out T> 
#else
        IHaveSelectedItems<T> 
#endif
        : IHaveSelectedItems
    {
        /// <summary>
        /// Selected items
        /// </summary>
        new IEnumerable<T> SelectedItems { get; }
    }

    /// <summary>
    /// Object that have multiple selected items
    /// </summary>
    public interface IHaveSelectedItems
    {
        /// <summary>
        /// Selected items
        /// </summary>
        IEnumerable SelectedItems { get; }
    }
}