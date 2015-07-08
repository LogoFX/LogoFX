// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces
{
    /// <summary>
    /// Object that have current item
    /// </summary>
    public interface IHaveCurrentItem
    {
        /// <summary>
        /// Current item
        /// </summary>
        /// <remarks>Usually synchronized with focus</remarks>
        object CurrentItem { get; }
    }
    /// <summary>
    /// Object that have current item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHaveCurrentItem<out T> : IHaveCurrentItem
    {
        /// <summary>
        /// Current item
        /// </summary>
        /// <remarks>Usually synchronized with focus</remarks>
        new T CurrentItem { get; }
    }

}