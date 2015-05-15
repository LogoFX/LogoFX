using System;

namespace LogoFX.UI.Navigation
{
    public interface INavigationBuilder
    {
        Type ConductorType { get; }

        bool IsRoot { get; }

        bool NotRemember { get; }

        object GetValue();
    }
}