// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Denotes model that wraps foreign object
    /// </summary>
    /// <typeparam name="T">type of object wrapped</typeparam>
    public interface IObjectModel<out T>:IModel
    {
        T Object { get; }
    }

    /// <summary>
    /// Denotes model that wraps any foreign object
    /// </summary>
    public interface IObjectModel : IObjectModel<object>
    {
    }
}