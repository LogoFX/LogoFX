namespace LogoFX.Practices.IoC
{
    public class NamedParameter : IParameter
    {
        public NamedParameter(string parameterName, object parameterValue)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }
    }
}