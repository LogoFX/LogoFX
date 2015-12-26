using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents view models collection.
    /// </summary>
    public sealed class ViewModelCollection : ObservableCollection<ViewModelBase>
    {
        /// <summary>
        /// Gets view model by the specified type name.
        /// </summary>
        /// <param name="type">The specified type name.</param>
        public ViewModelBase this[string type]
        {
            get 
            {
                return this.Single(item => item.GetType().Name == type);
            }
        }

        /// <summary>
        /// Gets the <see cref="ViewModelBase"/> with the specified type.
        /// </summary>
        /// <value>
        /// The <see cref="ViewModelBase"/>.
        /// </value>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ViewModelBase this[Type type]
        {
            get 
            {
                return this.Single(item => item.GetType() == type);
            }
        }
    }
}
