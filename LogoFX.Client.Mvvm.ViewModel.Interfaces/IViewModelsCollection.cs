using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace LogoFX.Client.Mvvm.ViewModel.Interfaces
{

#if SILVERLIGHT
    public interface IViewModelsCollection<T> : IEnumerable<T>, INotifyPropertyChanged, INotifyCollectionChanged where T : IViewModel
#else
    public interface IViewModelsCollection<out T> : IEnumerable<T>, INotifyPropertyChanged, INotifyCollectionChanged where T : IViewModel
#endif
    {
        /// <summary>
        /// Gets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value></value>
        T this[int index] { get; }
    }

    public interface IObjectViewModelsCollection : IViewModelsCollection<IObjectViewModel>
    {
    }


#if SILVERLIGHT 
    public interface IObjectViewModelsCollection<T> : IViewModelsCollection<T> where T:IObjectViewModel
#else
    public interface IObjectViewModelsCollection<out T> : IViewModelsCollection<T> where T:IObjectViewModel
#endif
    {
    }

}