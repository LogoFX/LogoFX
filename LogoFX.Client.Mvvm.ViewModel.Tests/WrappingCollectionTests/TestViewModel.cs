namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
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