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
        /// <summary>
        /// 
        /// </summary>
        object Value { get; set; } //data
        /// <summary>
        /// 
        /// </summary>
        string Name { get; } //meta
        /// <summary>
        /// 
        /// </summary>
        bool HaveDescription { get; } //meta
        /// <summary>
        /// 
        /// </summary>
        string Description { get; set; } //data
        /// <summary>
        /// 
        /// </summary>
        string DisplayName { get; } //data
    }
}
