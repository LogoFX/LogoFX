using System;
using System.Linq;
using Attest.Fake.Builders;
using Attest.Fake.Moq;
using Attest.Tests.Core;
using LogoFX.Client.Tests.EndToEnd.Shared;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Tests.EndToEnd.Modularity
{
    public abstract class ProvidersModuleBase<TContainerAdapter> : ICompositionModule<TContainerAdapter>
        where TContainerAdapter : IIocContainer
    {
        public void RegisterModule(TContainerAdapter iocContainer)
        {
            BuildersCollectionContext.DeserializeBuilders();
            OnRegisterProviders(iocContainer);
        }

        protected virtual void OnRegisterProviders(IIocContainer iocContainer)
        {

        }

        protected void RegisterAllBuilders<TProvider>(IIocContainer iocContainer, Func<FakeBuilderBase<TProvider>> defaultBuilderCreationFunc) where TProvider : class
        {
            var builders = BuildersCollectionContext.GetBuilders<TProvider>().ToArray();
            if (builders.Length == 0)
            {
                IntegrationTestsHelper<FakeFactory>.RegisterBuilder(iocContainer, defaultBuilderCreationFunc());
            }
            else
            {
                foreach (var builder in builders)
                {
                    IntegrationTestsHelper<FakeFactory>.RegisterBuilder(iocContainer, builder);
                }
            }

        }
    }
}