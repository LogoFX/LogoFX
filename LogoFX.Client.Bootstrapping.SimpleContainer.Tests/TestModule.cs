namespace LogoFX.Client.Bootstrapping.SimpleContainer.Tests
{
    internal interface ITestModule
    {
    }

    internal class TestModule : ITestModule
    {
        public string Name { get; set; }
    }   
}