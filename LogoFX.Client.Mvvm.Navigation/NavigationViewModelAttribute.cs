using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Used to mark the view models that are to be included in the navigation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NavigationViewModelAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the conductor.
        /// </summary>
        /// <value>
        /// The type of the conductor.
        /// </value>
        public Type ConductorType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is singleton.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is singleton; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether state should be remembered between the navigations.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state should be remembered between the navigations; otherwise, <c>false</c>.
        /// </value>
        public bool NotRemember { get; set; }
    }
}