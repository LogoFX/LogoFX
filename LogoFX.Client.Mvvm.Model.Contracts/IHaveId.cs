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
