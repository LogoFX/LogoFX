// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;
using System.Windows.Input;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents command with parameter after it has been setup with execution condition
    /// </summary>
    /// <typeparam name="TParameter">Type of command parameter</typeparam>
    /// <typeparam name="TCommand">Type of command</typeparam>
    public interface ICommandCondition<TParameter, out TCommand> where TCommand: ICommand
    {
        TCommand Do(Action<TParameter> execute);
    }

    /// <summary>
    /// Represents command after it has been setup with execution condition
    /// </summary>
    /// <typeparam name="TCommand">Type of command</typeparam>
    public interface ICommandCondition<out TCommand> where TCommand: ICommand
    {
        TCommand Do(Action execute);
    }
}
