// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

// This code is based on foundings in http://nroute.codeplex.com/

using System;

namespace LogoFX.UI.Commanding
{
    public class CommandEventArgs
		 : EventArgs
    {
        private readonly Object _commandParameter;

        public CommandEventArgs(Object commandParameter)
        {
            _commandParameter = commandParameter;
        }

        public Object CommandParameter
        {
            get { return _commandParameter; }
        }
    }
}
