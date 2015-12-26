using System;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using Microsoft.Practices.Unity;

namespace LogoFX.Client.Mvvm.ViewModelFactory.Unity
{
    /// <summary>
    /// Represents <see cref="IViewModelFactory"/> implementation using <see cref="UnityContainer"/> IoC container
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IUnityContainer _unityContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactory"/> class.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        public ViewModelFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
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
            return _unityContainer.Resolve<TViewModel>(new ParameterOverride("model", model));
        }

        /// <summary>
        /// Resolves the view model via inner lifetime scope while applying inner middelware.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="registerInnerMiddleware">The specified inner middleware.</param>
        /// <returns></returns>
        public TViewModel ResolveViaInnerLifetimeScope<TViewModel>(Action<IUnityContainer> registerInnerMiddleware)
        {
            var lifetimeScope = _unityContainer.CreateChildContainer();
            registerInnerMiddleware(lifetimeScope);
            lifetimeScope.RegisterType<IViewModelFactory, ViewModelFactory>(new HierarchicalLifetimeManager());
            var viewModel = lifetimeScope.Resolve<TViewModel>();
            return viewModel;
        }
    }
}
