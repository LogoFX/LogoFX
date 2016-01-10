using System;
using System.Linq;
using Attest.Fake.Builders;
using Attest.Fake.Registration;
using LogoFX.Client.Tests.EndToEnd.Shared;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Tests.EndToEnd.Modularity
{
    /// <summary>
    /// Base module for fake providers to be used in End-To-End tests.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <seealso cref="Solid.Practices.Modularity.ICompositionModule{TContainerAdapter}" />
    public abstract class ProvidersModuleBase<TContainer> : ICompositionModule<TContainer>
        where TContainer : IIocContainer
    {
        /// <summary>
        /// Registers composition module into IoC container
        /// </summary>
        /// <param name="iocContainer">IoC container</param>
        public void RegisterModule(TContainer iocContainer)
        {
            BuildersCollectionContext.DeserializeBuilders();
            OnRegisterProviders(iocContainer);            
        }

        /// <summary>
        /// Override this method to register application providers into the container.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        protected virtual void OnRegisterProviders(IIocContainer iocContainer)
        {

        }

        /// <summary>
        /// Registers all builders of the given provider type into the container.
        /// </summary>
        /// <typeparam name="TProvider">The type of the provider.</typeparam>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="defaultBuilderCreationFunc">The default builder creation function.</param>
        protected void RegisterAllBuilders<TProvider>(IIocContainer iocContainer, Func<FakeBuilderBase<TProvider>> defaultBuilderCreationFunc) where TProvider : class
        {
            var builders = BuildersCollectionContext.GetBuilders<TProvider>().ToArray();
            if (builders.Length == 0)
            {
                RegistrationHelper.RegisterBuilder(iocContainer, defaultBuilderCreationFunc());
            }
            else
            {
                foreach (var builder in builders)
                {
                    RegistrationHelper.RegisterBuilder(iocContainer, builder);
                }
            }
        }
    }
}