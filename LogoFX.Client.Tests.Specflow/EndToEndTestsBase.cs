using Solid.Practices.IoC;

namespace LogoFX.Client.Tests.SpecFlow
{
    /// <summary>
    /// Base class for client end-to-end tests.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    public abstract class EndToEndTestsBase<TContainer> :
        Attest.Tests.SpecFlow.EndToEndTestsBase<TContainer>
        where TContainer : IIocContainer, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestsBase{TContainer, TRootViewModel, TBootstrapper}"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        protected EndToEndTestsBase(TContainer container) : base(container)
        {
        }
    }
}