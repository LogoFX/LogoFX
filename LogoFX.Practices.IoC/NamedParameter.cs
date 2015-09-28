namespace LogoFX.Practices.IoC
{
    /// <summary>
    /// Represents named parameter for dependency resolution
    /// </summary>
    public class NamedParameter : IParameter
    {
        public NamedParameter(string parameterName, object parameterValue)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public string ParameterName { get; private set; }
        public object ParameterValue { get; private set; }
    }
}