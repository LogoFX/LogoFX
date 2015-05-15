using System;
using Solid.Practices.Modularity;

namespace LogoFX.UI.Modularity
{
    public interface IUiModule : ICompositionModule
    {        
        string Name { get; }

        int Order { get; }                
    }

    public interface IUiModule<out TRootViewModel> : IUiModule
    {
        TRootViewModel RootViewModel { get; }

        Type RootModelType { get; }
    }
}
