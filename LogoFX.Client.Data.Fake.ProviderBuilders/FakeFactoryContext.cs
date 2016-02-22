using Attest.Fake.Core;

namespace LogoFX.Client.Data.Fake.ProviderBuilders
{
    public static class FakeFactoryContext
    {
        private static IFakeFactory _fakeFactory;

        public static IFakeFactory Current
        {
            get { return _fakeFactory; }
            set { _fakeFactory = value; }
        }
    }
}
