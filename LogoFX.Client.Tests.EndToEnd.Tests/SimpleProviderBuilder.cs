using System;
using System.Collections.Generic;
using Attest.Fake.Builders;
using Attest.Fake.Moq;
using Attest.Fake.Setup;

namespace LogoFX.Client.Tests.EndToEnd.Tests
{
    [Serializable]
    public class SimpleItemDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public interface ISimpleProvider
    {
        IEnumerable<SimpleItemDto> GetSimpleItems();
    }

    [Serializable]
    class SimpleProviderBuilder : FakeBuilderBase<ISimpleProvider>
    {
        private readonly List<SimpleItemDto> _warehouseItemsStorage = new List<SimpleItemDto>();

        private SimpleProviderBuilder() :base(new FakeFactory())
        {

        }

        public static SimpleProviderBuilder CreateBuilder()
        {
            return new SimpleProviderBuilder();
        }

        public void WithWarehouseItems(IEnumerable<SimpleItemDto> warehouseItems)
        {
            _warehouseItemsStorage.Clear();
            _warehouseItemsStorage.AddRange(warehouseItems);
        }

        protected override void SetupFake()
        {
            var initSetup = ServiceCall<ISimpleProvider>.CreateServiceCall(FakeService);

            var setup = initSetup
                .AddMethodCall(MethodCallWithResult0<ISimpleProvider, IEnumerable<SimpleItemDto>>
                        .CreateMethodCall(t => t.GetSimpleItems())
                        .BuildCallbacks(r => r.Complete(GetSimpleItems())));

            setup.SetupService();
        }

        private IEnumerable<SimpleItemDto> GetSimpleItems()
        {
            return _warehouseItemsStorage;
        }
    }
}
