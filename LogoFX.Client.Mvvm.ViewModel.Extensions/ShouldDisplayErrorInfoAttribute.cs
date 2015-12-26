using System;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// This attribute is used to determine 
    /// whether a property error information 
    /// should be displayed. Use inside view models.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ShouldDisplayErrorInfoAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the associated property error should be displayed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if should be displayed; otherwise, <c>false</c>.
        /// </value>
        public bool ShouldDisplay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShouldDisplayErrorInfoAttribute"/> class.
        /// </summary>
        /// <param name="shouldDisplay">if set to <c>true</c> [should display].</param>
        public ShouldDisplayErrorInfoAttribute(bool shouldDisplay)
        {
            ShouldDisplay = shouldDisplay;
        }
    }
}
