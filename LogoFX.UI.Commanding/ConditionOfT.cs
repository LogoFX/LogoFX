#region Copyright
// Partial Copyright (c) LogoUI Software Solutions LTD
// Author: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.
#endregion

using System;

namespace LogoFX.UI.Commanding
{
    public class Condition<T> : ICommandCondition<T, ActionCommand<T>>
    {
        private readonly Func<T, bool> _canExecute;

        public Condition(Func<T, bool> canExecute)
        {
            _canExecute = canExecute;
        }

        public ActionCommand<T> Do(Action<T> execute)
        {
            return new ActionCommand<T>(execute, _canExecute);
        }
    }
}
