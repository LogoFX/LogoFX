using System;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Modularity
{
    /// <summary>
    /// Represents user interface composition module
    /// </summary>
    public interface IUiModule : ICompositionModule
    {
        /// <summary>
        /// Module ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Module display name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Module display order
        /// </summary>
        int Order { get; }                
    }

    /// <summary>
    /// Respresents user interface composition module with root view model
    /// </summary>
    /// <typeparam name="TRootViewModel">Type of Root ViewModel</typeparam>
    public interface IUiModule<out TRootViewModel> : IUiModule where TRootViewModel : IRootViewModel
    {
        /// <summary>
        /// Root ViewModel
        /// </summary>
        TRootViewModel RootViewModel { get; }

        /// <summary>
        /// Explicit type of Root ViewModel
        /// </summary>
        Type RootModelType { get; }
    }
}
