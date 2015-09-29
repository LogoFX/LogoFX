// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

// This code is based on foundings in http://nroute.codeplex.com/

using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Command implementation which allows custom notifications, composite execution predicates, etc.
    /// </summary>
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

        /// <summary>
        /// Specifies the condition that must be satisfied for command execution
        /// </summary>
        /// <param name="condition">Condition to be satisfied</param>
        /// <returns>Command condition</returns>
        public static ICommandCondition<ActionCommand> When(Func<bool> condition)
        {
            return new Condition(condition);
        }

        /// <summary>
        /// Specifies the action to be run on command execution
        /// </summary>
        /// <param name="execute">Action to be run</param>
        /// <returns>Extended command</returns>
        public static IExtendedCommand Do(Action execute)
        {
            return new ActionCommand(execute, () => true);
        }
    }
}
