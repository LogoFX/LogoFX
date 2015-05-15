using System;
using LogoFX.UI.ViewModels.Interfaces;

namespace LogoFX.UI.ViewModels.ViewModelFactory.SimpleContainer
{
    class ViewModelFactory : IViewModelFactory
    {
        private readonly Practices.IoC.SimpleContainer _simpleContainer;

        public ViewModelFactory(Practices.IoC.SimpleContainer simpleContainer)
        {
            _simpleContainer = simpleContainer;
        }

        public TViewModel CreateModelWrapper<TModel, TViewModel>(TModel model) where TViewModel : IModelWrapper<TModel>
        {
            throw new NotImplementedException("Simple Container does not support parameters during resolution");
        }
    }
}
