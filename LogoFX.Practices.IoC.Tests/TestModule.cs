namespace LogoFX.Practices.IoC.Tests
{
    interface ITestModule
    {
        string Name { get; set; }
    }

    class TestModule : ITestModule
    {
        public string Name { get; set; }
    }
}
