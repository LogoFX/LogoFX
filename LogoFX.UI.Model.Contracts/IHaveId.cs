// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

using System;

namespace LogoFX.UI.Model.Contracts
{
    public interface IHaveId<T> where T:IEquatable<T>
    {
        T Id { get; set; }
    }
}
