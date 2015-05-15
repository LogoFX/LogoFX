using System;
using Autofac;
using LogoFX.UI.ViewModels.Interfaces;

namespace LogoFX.UI.ViewModels.ViewModelFactory.Autofac
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IContainer _container;

        public ViewModelFactory(IContainer container)
        {
            _container = container;
        }

        public TViewModel CreateModelWrapper<TModel, TViewModel>(TModel model) where TViewModel : IModelWrapper<TModel>
        {
            return _container.Resolve<TViewModel>(new NamedParameter("model", model));
        }

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
