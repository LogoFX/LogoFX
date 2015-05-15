// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.UI.ViewModels.Interfaces
{
    /// <summary>
    /// Object that have one selected item
    /// </summary>
    /// <typeparam name="T">type of selected item supported</typeparam>
    public interface IHaveSelectedItem<out T>:IHaveSelectedItem
    {
        /// <summary>
        /// Selected item
        /// </summary>
        new T SelectedItem { get; }
    }

    /// <summary>
    /// Object that have one selected item
    /// </summary>
    public interface IHaveSelectedItem
    {
        /// <summary>
        /// Selected item
        /// </summary>
        object SelectedItem { get; }
    }
}