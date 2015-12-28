namespace LogoFX.Practices.IoC
{
    /// <summary>
    /// Represents parameter for dependency resolution
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <value>
        /// The parameter value.
        /// </value>
        object ParameterValue { get; }
    }
}
