// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System.ComponentModel;

namespace LogoFX.Client.Commanding
{
    public interface IActionCommand
         : IReverseCommand, IReceiveEvent, IExtendedCommand, INotifyPropertyChanged
    {
        bool IsActive { get; set; }

        void RequeryCanExecute();
    }
}
