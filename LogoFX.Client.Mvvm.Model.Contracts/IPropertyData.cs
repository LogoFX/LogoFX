using System;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents property data
    /// </summary>
    [Obsolete]
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
