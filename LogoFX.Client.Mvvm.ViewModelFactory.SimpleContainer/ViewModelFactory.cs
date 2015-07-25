using LogoFX.Client.Mvvm.ViewModel.Interfaces;
using LogoFX.Practices.IoC;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly ExtendedSimpleContainer _simpleContainer;

        public ViewModelFactory(ExtendedSimpleContainer simpleContainer)
        {
            _simpleContainer = simpleContainer;
        }

        public TViewModel CreateModelWrapper<TModel, TViewModel>(TModel model) where TViewModel : IModelWrapper<TModel>
        {
            return (TViewModel) _simpleContainer.GetInstance(typeof (TViewModel), null,
                new IParameter[] {new NamedParameter("model", model)});
        }
    }
}
