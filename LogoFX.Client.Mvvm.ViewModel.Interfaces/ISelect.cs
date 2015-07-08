// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces
{

    /// <summary>
    /// Object that can select
    /// </summary>
    public interface ISelect
    {
        /// <summary>
        /// Selects the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="notify"></param>
        /// <returns>true if succeeded, otherwise false</returns>
        bool Select(object item, bool notify = true);
    }
}
