namespace LogoFX.UI.ViewModels.Tests.WrappingCollectionTests
{
    class TestViewModel
    {
        public TestModel Model { get; private set; }

        public TestViewModel(TestModel model)
        {
            Model = model;
        }
    }
}