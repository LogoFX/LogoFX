using SimpleInjector;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleInjector
{
    /// <summary>
    /// Contains extensios for <see cref="ContainerOptions"/>
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Registers the parameter convention for the specified container options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="convention">The convention.</param>
        public static void RegisterParameterConvention(this ContainerOptions options,
            IParameterConvention convention)
        {
            RegisterParameterConventionInternal(options, convention);
        }

        private static void RegisterParameterConventionInternal(ContainerOptions options, IParameterConvention convention)
        {
            options.DependencyInjectionBehavior = new ConventionDependencyInjectionBehavior(
                options.DependencyInjectionBehavior, convention);
        }

        /// <summary>
        /// Registers the model parameter convention for the specified container options.
        /// </summary>
        /// <param name="options">The options.</param>
        public static void RegisterModelParameterConvention(this ContainerOptions options)
        {
            RegisterParameterConventionInternal(options, new ModelParameterConvention());
        }
    }
}