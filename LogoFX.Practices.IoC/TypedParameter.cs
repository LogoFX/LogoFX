using System;

namespace LogoFX.Practices.IoC
{
    /// <summary>
    /// Represents typed parameter for dependency resolution
    /// </summary>
    public class TypedParameter : IParameter
    {
        public TypedParameter(Type parameterType, object parameterValue)
        {
            ParameterType = parameterType;
            ParameterValue = parameterValue;
        }

        public Type ParameterType { get; private set; }
        public object ParameterValue { get; private set; }
    }
}