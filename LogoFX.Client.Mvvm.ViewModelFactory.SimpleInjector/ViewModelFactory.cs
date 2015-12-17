using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleInjector
{
    static class Consts
    {
        internal static string ModelParameterName = "model";
    }

    public class ViewModelFactory : IViewModelFactory
    {
        private readonly Container _container;
        private readonly object _syncObject = new object();

        public ViewModelFactory(Container container)
        {
            _container = container;                       
        }

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
   
    public interface IParameterConvention
    {
        bool CanResolve(InjectionTargetInfo target);
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
