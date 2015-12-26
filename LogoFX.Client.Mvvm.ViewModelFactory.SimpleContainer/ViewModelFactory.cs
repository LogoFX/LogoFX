using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Practices.IoC;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleContainer
{
    /// <summary>
    /// Represents <see cref="IViewModelFactory"/> implementation using <see cref="ExtendedSimpleContainer"/> IoC container
    /// </summary>
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly ExtendedSimpleContainer _simpleContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactory"/> class.
        /// </summary>
        /// <param name="simpleContainer">The simple container.</param>
        public ViewModelFactory(ExtendedSimpleContainer simpleContainer)
        {
            _simpleContainer = simpleContainer;
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
            return (TViewModel) _simpleContainer.GetInstance(typeof (TViewModel), null,
                new IParameter[] {new NamedParameter("model", model)});
        }
    }
}
