// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

// This code is based on foundings in http://nroute.codeplex.com/

using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    public class ActionCommand
         : CommandBase
    {
        private readonly Func<bool> _canExecuteHandler;
        private readonly Action _executeHandler;

        public ActionCommand(Action executeHandler)
            : this(executeHandler, null, true) { }

        public ActionCommand(Action executeHandler, bool isActive)
            : this(executeHandler, null, isActive) { }

        public ActionCommand(Action executeHandler, Func<bool> canExecuteHandler)
            : this(executeHandler, canExecuteHandler, true) { }

        public ActionCommand(Action executeHandler, Func<bool> canExecuteHandler, bool isActive)
            : base(isActive)
        {
            Guard.ArgumentNotNull(executeHandler, "executeHandler");

            _executeHandler = executeHandler;
            _canExecuteHandler = canExecuteHandler;
        }

        #region Overrides

        protected override bool OnCanExecute()
        {
            return _canExecuteHandler != null ? _canExecuteHandler() : true;
        }

        protected override void OnExecute()
        {
            _executeHandler();
        }

        #endregion

        public static ICommandCondition<ActionCommand> When(Func<bool> condition)
        {
            return new Condition(condition);
        }

        public static IExtendedCommand Do(Action execute)
        {
            return new ActionCommand(execute, () => true);
        }

    }
}
