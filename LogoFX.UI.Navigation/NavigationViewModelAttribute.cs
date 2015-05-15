using System;

namespace LogoFX.UI.Navigation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NavigationViewModelAttribute : Attribute
    {
        public Type ConductorType { get; set; }

        public bool IsSingleton { get; set; }

        public bool NotRemember { get; set; }
    }
}