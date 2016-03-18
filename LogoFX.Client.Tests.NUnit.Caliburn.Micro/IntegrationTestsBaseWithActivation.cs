using Attest.Testing.Core;
using Caliburn.Micro;
using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.NUnit.Caliburn.Micro
{
    /// <summary>
    /// Base class for Caliburn.Micro and Nunit-based 
    /// client integration tests which involve root object activation.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>    
    public abstract class IntegrationTestsBaseWithActivation<TContainer, TRootViewModel, TBootstrapper> :
        IntegrationTestsBase<TContainer, TRootViewModel, TBootstrapper>
        where TContainer : IIocContainer, new() where TRootViewModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestsBaseWithActivation{TContainer, TRootViewModel, TBootstrapper}"/> class.
        /// </summary>
        /// <param name="resolutionStyle">The resolution style.</param>
        protected IntegrationTestsBaseWithActivation(InitializationParametersResolutionStyle resolutionStyle = InitializationParametersResolutionStyle.PerRequest)
            :base(resolutionStyle)
        {
            
        }

        /// <summary>
        /// Provides additional opportunity to modify the root object immediately after is has been created
        /// </summary>
        /// <param name="rootObject">Newly created root object</param>
        /// <returns>
        /// Modified root object
        /// </returns>
        protected override TRootViewModel CreateRootObjectOverride(TRootViewModel rootObject)
        {
            ScreenExtensions.TryActivate(rootObject);
            return rootObject;
        }        
    }
}