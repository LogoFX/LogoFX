namespace LogoFX.UI.ViewModels.Tests.WrappingCollectionTests
{
    class TestModel
    {
        public TestModel(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}