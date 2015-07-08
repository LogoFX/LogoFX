// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces
{
    /// <summary>
    /// Item that can be current
    /// </summary>
    public interface ICurrent
    {
        /// <summary>
        /// If current
        /// </summary>
        bool IsCurrent { get; set; }
    }
}