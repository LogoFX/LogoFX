// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents object with an identifier
    /// </summary>
    /// <typeparam name="T">Type of identifier</typeparam>
    public interface IHaveId<T> where T:IEquatable<T>
    {
        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        T Id { get; set; }
    }
}
