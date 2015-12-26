using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleInjector
{
    static class Consts
    {
        internal static string ModelParameterName = "model";
    }

    /// <summary>
    /// Represents <see cref="IViewModelFactory" /> implementation using <see cref="Container"/>/>
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly Container _container;
        private readonly object _syncObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ViewModelFactory(Container container)
        {
            _container = container;                       
        }

        /// <summary>
        /// Creates the view model which has capabilities of a model wrapper.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public TViewModel CreateModelWrapper<TModel, TViewModel>(TModel model) where TViewModel : IModelWrapper<TModel>
        {
            var localDataStoreSlot = Thread.AllocateNamedDataSlot(Consts.ModelParameterName);
            Thread.SetData(localDataStoreSlot, model);

            lock (_syncObject)
            {
                return (TViewModel)_container.GetInstance(typeof(TViewModel));
            }
            
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

        [DebuggerStepThrough]
        public bool CanResolve(InjectionTargetInfo target)
        {            
            bool resolvable = target.Name == Consts.ModelParameterName;
            if (resolvable)
            {
                _modelType = target.TargetType;
            }
            return resolvable;
        }

        [DebuggerStepThrough]
        public Expression BuildExpression(InjectionConsumerInfo consumer)
        {            
            var localDataStoreSlot = Thread.GetNamedDataSlot(Consts.ModelParameterName);
            string model = Thread.GetData(localDataStoreSlot).ToString();
            return Expression.Constant(model, _modelType);
        }
    }

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
