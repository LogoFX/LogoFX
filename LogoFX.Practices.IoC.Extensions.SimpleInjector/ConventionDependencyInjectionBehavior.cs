using System.Diagnostics;
using System.Linq.Expressions;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace LogoFX.Practices.IoC.Extensions.SimpleInjector
{
    internal class ConventionDependencyInjectionBehavior : IDependencyInjectionBehavior
    {
        private readonly IDependencyInjectionBehavior decoratee;
        private readonly IParameterConvention convention;

        public ConventionDependencyInjectionBehavior(
            IDependencyInjectionBehavior decoratee, IParameterConvention convention)
        {
            this.decoratee = decoratee;
            this.convention = convention;
        }

        [DebuggerStepThrough]
        public Expression BuildExpression(InjectionConsumerInfo consumer)
        {
            return this.convention.CanResolve(consumer.Target)
                ? this.convention.BuildExpression(consumer)
                : this.decoratee.BuildExpression(consumer);
        }

        [DebuggerStepThrough]
        public void Verify(InjectionConsumerInfo consumer)
        {
            if (!this.convention.CanResolve(consumer.Target))
            {
                this.decoratee.Verify(consumer);
            }
        }
    }
}