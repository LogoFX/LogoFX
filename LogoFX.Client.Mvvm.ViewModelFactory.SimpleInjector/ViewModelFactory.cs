using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Practices.IoC.Extensions.SimpleInjector;
using SimpleInjector;

namespace LogoFX.Client.Mvvm.ViewModelFactory.SimpleInjector
{    
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
            using (new ObjectResolver(model))
            {
                lock (_syncObject)
                {
                    return (TViewModel)_container.GetInstance(typeof(TViewModel));
                }
            }
        }
    }    
}
