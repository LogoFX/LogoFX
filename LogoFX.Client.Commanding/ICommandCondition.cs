// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;
using System.Windows.Input;

namespace LogoFX.Client.Commanding
{
    public interface ICommandCondition<T, out TRet> where TRet:ICommand
    {
        TRet Do(Action<T> execute);
    }

    public interface ICommandCondition<out TRet> where TRet : ICommand
    {

        TRet Do(Action execute);
    }
}
