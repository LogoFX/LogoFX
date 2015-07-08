using System;

namespace LogoFX.Client.Mvvm.Navigation
{
    public interface INavigationBuilder
    {
        Type ConductorType { get; }

        bool IsRoot { get; }

        bool NotRemember { get; }

        object GetValue();
    }
}