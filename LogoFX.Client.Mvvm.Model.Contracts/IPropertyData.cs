// Partial Copyright (c) LogoUI Software Solutions LTD
// Autor: Vladislav Spivak
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licences and credits.

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents property data
    /// </summary>
    public interface IPropertyData
    {
        object Value { get; set; } //data
        string Name { get; } //meta
        bool HaveDescription { get; } //meta
        string Description { get; set; } //data
        string DisplayName { get; } //data
    }
}
