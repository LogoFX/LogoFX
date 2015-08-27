using LogoFX.Client.Mvvm.Model;

namespace LogoFX.Client.Mvvm.ViewModel.Tests.WrappingCollectionTests
{
    class TestModel : Model<int>
    {
        public TestModel(int id)
        {
            Id = id;
        }        

        public override int GetHashCode()
        {
            return Id;
        }
    }
}