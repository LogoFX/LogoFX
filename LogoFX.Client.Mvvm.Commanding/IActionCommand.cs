// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents a command that supports various requery options 
    /// and is able to receive events on property and collection notifications
    /// </summary>
    public interface IActionCommand
         : IReverseCommand, IReceiveEvent, IExtendedCommand, INotifyPropertyChanged
    {
        bool IsActive { get; set; }

        void RequeryCanExecute();
    }
}
