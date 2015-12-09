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
        public bool ShouldDisplay { get; set; }

        public ShouldDisplayErrorInfoAttribute(bool shouldDisplay)
        {
            ShouldDisplay = shouldDisplay;
        }
    }
}
