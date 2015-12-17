using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// Used for redirection scenarios
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]    
    public sealed class NavigationSynonymAttribute : Attribute
    {
        public NavigationSynonymAttribute(Type synonymType)
        {
            SynonymType = synonymType;
        }

        public Type SynonymType { get; private set; }
    }
}