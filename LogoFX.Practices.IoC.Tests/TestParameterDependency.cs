namespace LogoFX.Practices.IoC.Tests
{
    interface ITestNamedParameterDependency
    {
        string Model { get; }
    }

    class TestNamedParameterDependency : ITestNamedParameterDependency
    {
        public TestNamedParameterDependency(string model)
        {
            Model = model;
        }

        public string Model { get; private set; }
    }

    interface ITestTypedParameterDependency
    {
        int Value { get; }
    }

    class TestTypedParameterDependency : ITestTypedParameterDependency
    {
        public TestTypedParameterDependency(int val)
        {
            Value = val;
        }

        public int Value { get; private set; }
    }
}