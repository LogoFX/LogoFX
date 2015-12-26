using System.ComponentModel;
using LogoFX.Client.Mvvm.ViewModel.Interfaces;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Represents view model creator service.
    /// </summary>
    public interface IViewModelCreatorService
    {
        /// <summary>
        /// Creates the view model by its type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns></returns>
        TViewModel CreateViewModel<TViewModel>() where TViewModel : class, INotifyPropertyChanged;

        /// <summary>
        /// Creates the model wrapper.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TViewModel">The type of the model wrapper.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        TViewModel CreateViewModel<TModel, TViewModel>(TModel model)
            where TViewModel : class, INotifyPropertyChanged, IModelWrapper<TModel>;
    }

    /// <summary>
    /// Represents view model creator service.
    /// </summary>
    public class ViewModelCreatorService : IViewModelCreatorService
    {
        private readonly IIocContainer _container;
        private readonly IViewModelFactory _viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelCreatorService"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        public ViewModelCreatorService(IIocContainer container, IViewModelFactory viewModelFactory)
        {
            _container = container;
            _viewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// Creates the view model by its type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns></returns>
        public TViewModel CreateViewModel<TViewModel>() where TViewModel : class, INotifyPropertyChanged
        {
            return _container.Resolve<TViewModel>();
        }

        /// <summary>
        /// Creates the model wrapper.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TViewModel">The type of the model wrapper.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public TViewModel CreateViewModel<TModel, TViewModel>(TModel model) where TViewModel : class, INotifyPropertyChanged, IModelWrapper<TModel>
        {
            return _viewModelFactory.CreateModelWrapper<TModel, TViewModel>(model);
        }
    }
}
