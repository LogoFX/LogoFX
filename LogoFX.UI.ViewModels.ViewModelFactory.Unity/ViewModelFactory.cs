using System;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;
using Microsoft.Practices.Unity;

namespace LogoFX.Client.Mvvm.ViewModel.ViewModelFactory.Unity
{
    class ViewModelFactory : IViewModelFactory
    {
        private readonly IUnityContainer _unityContainer;

        public ViewModelFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public TViewModel CreateModelWrapper<TModel, TViewModel>(TModel model) where TViewModel : IModelWrapper<TModel>
        {
            return _unityContainer.Resolve<TViewModel>(new ParameterOverride("model", model));
        }

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
