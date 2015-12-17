using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Navigation builder which allows exposes the navigation object properties
    /// </summary>
    public interface INavigationBuilder
    {
        /// <summary>
        /// Gets the type of the conductor.
        /// </summary>
        /// <value>
        /// The type of the conductor.
        /// </value>
        Type ConductorType { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is root.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is root; otherwise, <c>false</c>.
        /// </value>
        bool IsRoot { get; }

        /// <summary>
        /// Gets a value indicating whether the state should be preserved between navigations
        /// </summary>
        /// <value>
        ///   <c>true</c> if the state should be preserved between navigations; otherwise, <c>false</c>.
        /// </value>
        bool NotRemember { get; }

        /// <summary>
        /// Gets the navigation target
        /// </summary>
        /// <returns>Navigation target</returns>
        object GetValue();
    }
}