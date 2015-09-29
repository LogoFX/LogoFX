// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

// This code is based on foundings in http://nroute.codeplex.com/

using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Generic implementation of <see cref="ActionCommand"/>
    /// </summary>
    /// <typeparam name="T">Type of command parameter</typeparam>
    public class ActionCommand<T>
         : CommandBase<T>
    {

        #region Declarations

        private readonly Func<T, bool> _canExecuteHandler;
        private readonly Action<T> _executeHandler;

        #endregion

        public ActionCommand(Action<T> executeHandler)
            : this(executeHandler, null, true) { }

        public ActionCommand(Action<T> executeHandler, bool isActive)
            : this(executeHandler, null, isActive) { }

        public ActionCommand(Action<T> executeHandler, Func<T, bool> canExecuteHandler)
            : this(executeHandler, canExecuteHandler, true) { }

        public ActionCommand(Action<T> executeHandler, Func<T, bool> canExecuteHandler, bool isActive)
            : base(isActive)
        {
            Guard.ArgumentNotNull(executeHandler, "executeHandler");

            _executeHandler = executeHandler;
            _canExecuteHandler = canExecuteHandler;
        }

        #region Override

        protected override bool OnCanExecute(T parameter)
        {
            return _canExecuteHandler != null ? _canExecuteHandler(parameter) : true;
        }

        protected override void OnExecute(T parameter)
        {
            _executeHandler(parameter);
        }

        #endregion

        /// <summary>
        /// Specifies the condition that must be satisfied for command execution
        /// </summary>
        /// <param name="condition">Condition to be satisfied</param>
        /// <returns>Command condition</returns>
        public static ICommandCondition<T, ActionCommand<T>> When(Func<T, bool> condition)
        {
            return new Condition<T>(condition);
        }

        /// <summary>
        /// Specifies the action to be run on command execution
        /// </summary>
        /// <param name="execute">Action to be run</param>
        /// <returns>Extended command</returns>
        public static IExtendedCommand Do(Action<T> execute)
        {
            return new ActionCommand<T>(execute, o => true);
        }
    }
}
