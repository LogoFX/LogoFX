using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Used for redirection scenarios
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]    
    public sealed class NavigationSynonymAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationSynonymAttribute"/> class.
        /// </summary>
        /// <param name="synonymType">Type of the synonym.</param>
        public NavigationSynonymAttribute(Type synonymType)
        {
            SynonymType = synonymType;
        }

        /// <summary>
        /// Gets the type of the synonym.
        /// </summary>
        /// <value>
        /// The type of the synonym.
        /// </value>
        public Type SynonymType { get; private set; }
    }
}