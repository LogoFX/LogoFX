using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents screen object view model with custom error display logic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ErrorInfoViewModel<T> : ScreenObjectViewModel<T>, IDataErrorInfo
        where T : IDataErrorInfo, IHaveErrors
    {
        private readonly ConcurrentDictionary<string, bool> _isPropertyChanged = new ConcurrentDictionary<string, bool>();
        private readonly HashSet<string> _interestingProperties = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInfoViewModel{T}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected ErrorInfoViewModel(T model) : base(model)
        {
            var properties = GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                var shouldDisplayAttributes =
                    propertyInfo.GetCustomAttributes(typeof(ShouldDisplayErrorInfoAttribute), true)
                        .OfType<ShouldDisplayErrorInfoAttribute>().ToArray();
                if (shouldDisplayAttributes.Length == 1)
                {
                    var shouldDisplayAttribute = shouldDisplayAttributes[0];
                    if (shouldDisplayAttribute.ShouldDisplay)
                    {
                        _interestingProperties.Add(propertyInfo.Name);
                        _isPropertyChanged.TryAdd(propertyInfo.Name, false);
                    }
                }
            }

            PropertyChanged += WeakDelegate.From(OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var propertyName = propertyChangedEventArgs.PropertyName;
            if (_interestingProperties.Contains(propertyName))
            {
                _isPropertyChanged.TryUpdate(propertyChangedEventArgs.PropertyName, true, false);
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error
        {
            get
            {
                return Model.Error;
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public string this[string columnName]
        {
            get
            {
                bool value;
                var couldGetValue = _isPropertyChanged.TryGetValue(columnName, out value);
                return couldGetValue && value ? Model[columnName] : string.Empty;
            }
        }
    }
}