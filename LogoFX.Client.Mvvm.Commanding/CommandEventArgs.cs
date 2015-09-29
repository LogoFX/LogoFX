// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

// This code is based on foundings in http://nroute.codeplex.com/

using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    public class CommandEventArgs
		 : EventArgs
    {
        private readonly object _commandParameter;

        public CommandEventArgs(object commandParameter)
        {
            _commandParameter = commandParameter;
        }

        public object CommandParameter
        {
            get { return _commandParameter; }
        }
    }
}
