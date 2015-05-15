using System;

namespace LogoFX.UI.Navigation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class NavigationSynonymAttribute : Attribute
    {
        public NavigationSynonymAttribute(Type synonimType)
        {
            SynonimType = synonimType;
        }

        public Type SynonimType { get; private set; }
    }
}