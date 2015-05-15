// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.


// This code is based on foundings in http://nroute.codeplex.com/

using System;

namespace LogoFX.UI.Commanding
{
    public class Condition : ICommandCondition<ActionCommand>
    {
        private readonly Func<bool> _canExecute;

        public Condition(Func<bool> canExecute)
        {
            _canExecute = canExecute;
        }

        public ActionCommand Do(Action execute)
        {
            return new ActionCommand(execute, _canExecute);
        }
    }
}
