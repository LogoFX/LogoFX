using System;
using Autofac;
using Autofac.Core;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModelFactory.Autofac
{
    /// <summary>
    /// Represents <see cref="IViewModelFactory"/> implementation using <see cref="Container"/> IoC container
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ViewModelFactory(IContainer container)
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
            return _container.Resolve<TViewModel>(new NamedParameter("model", model));
        }

        /// <summary>
        /// Resolves the view model via inner lifetime scope while applying inner middelware.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="registerInnerMiddleware">The specified inner middleware.</param>
        /// <returns></returns>
        public TViewModel ResolveViaInnerLifetimeScope<TViewModel>(Action<ContainerBuilder> registerInnerMiddleware)
        {
            var lifetimeScope = _container.BeginLifetimeScope(r =>
            {
                registerInnerMiddleware(r);
                r.RegisterType<IViewModelFactory>().As<ViewModelFactory>().InstancePerLifetimeScope();
            });            
            var viewModel = lifetimeScope.Resolve<TViewModel>();
            return viewModel;
        }
    }
}
