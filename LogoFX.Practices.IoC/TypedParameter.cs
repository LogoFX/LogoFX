using System;

namespace LogoFX.Practices.IoC
{
    /// <summary>
    /// Represents typed parameter for dependency resolution
    /// </summary>
    public class TypedParameter : IParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypedParameter"/> class.
        /// </summary>
        /// <param name="parameterType">Type of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        public TypedParameter(Type parameterType, object parameterValue)
        {
            ParameterType = parameterType;
            ParameterValue = parameterValue;
        }

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        /// <value>
        /// The type of the parameter.
        /// </value>
        public Type ParameterType { get; private set; }

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <value>
        /// The parameter value.
        /// </value>
        public object ParameterValue { get; private set; }
    }
}