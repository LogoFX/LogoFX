using SimpleInjector;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleInjector
{
    public static class ContainerExtensions
    {
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

        public static void RegisterModelParameterConvention(this ContainerOptions options)
        {
            RegisterParameterConventionInternal(options, new ModelParameterConvention());
        }
    }
}