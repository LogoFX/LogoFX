using System;
using System.Linq.Expressions;
using System.Threading;
using SimpleInjector;

namespace LogoFX.Practices.IoC.Extensions.SimpleInjector
{
    /// <summary>
    /// Contains extensions for <see cref="ContainerOptions"/>
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

    /// <summary>
    /// Represents parameter convention used for registration inside the <see cref="Container"/>
    /// </summary>
    public interface IParameterConvention
    {
        /// <summary>
        /// Determines whether this instance can resolve the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        bool CanResolve(InjectionTargetInfo target);

        /// <summary>
        /// Builds the expression.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <returns></returns>
        Expression BuildExpression(InjectionConsumerInfo consumer);
    }

    class ModelParameterConvention : IParameterConvention
    {
        private Type _modelType;
        
        public bool CanResolve(InjectionTargetInfo target)
        {
            bool resolvable = target.Name == Consts.ModelParameterName;
            if (resolvable)
            {
                _modelType = target.TargetType;
            }
            return resolvable;
        }
        
        public Expression BuildExpression(InjectionConsumerInfo consumer)
        {
            var localDataStoreSlot = Thread.GetNamedDataSlot(Consts.ModelParameterName);
            var model = Thread.GetData(localDataStoreSlot);
            return Expression.Constant(model, _modelType);
            //var lambda =
            //    Expression.Lambda<Func<object>>(
            //        Expression.Constant(Thread.GetData(Thread.GetNamedDataSlot(Consts.ModelParameterName)), _modelType));
            //return lambda;

        }
    }
}
