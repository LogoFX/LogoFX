namespace LogoFX.Practices.IoC.Tests
{
    interface ITestParameterDependency
    {
        string Model { get; }
    }

    class TestParameterDependency : ITestParameterDependency
    {
        public TestParameterDependency(string model)
        {
            Model = model;
        }

        public string Model { get; private set; }
    }
}