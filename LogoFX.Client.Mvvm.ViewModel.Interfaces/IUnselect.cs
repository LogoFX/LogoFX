// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

 namespace LogoFX.Client.Mvvm.ViewModel.Interfaces
{
    /// <summary>
    /// Denotes object that have unselection facility 
    /// </summary>
    public interface IUnselect
    {
        /// <summary>
        /// Unselects object
        /// </summary>
        /// <param name="item"></param>
        /// <param name="notify"></param>
        /// <returns>true if succeeded, otherwise false</returns>
        bool Unselect(object item, bool notify = true);

        void ClearSelection();
    }
}
