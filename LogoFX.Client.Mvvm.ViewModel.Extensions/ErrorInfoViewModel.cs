using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract class ErrorInfoViewModel<T> : ScreenObjectViewModel<T>, IDataErrorInfo
        where T : IDataErrorInfo
    {
        private readonly ConcurrentDictionary<string, bool> _isPropertyChanged = new ConcurrentDictionary<string, bool>();
        private readonly HashSet<string> _interestingProperties = new HashSet<string>();

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

        public string Error
        {
            get
            {
                return Model.Error;
            }
        }

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