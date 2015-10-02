using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    partial class Model<T>
    {
        private readonly Dictionary<string, string> _externalErrors =
            new Dictionary<string, string>();

        private void InitErrorListener()
        {
            ListenToPropertyChange();
        }

        private void ListenToPropertyChange()
        {
            PropertyChanged += WeakDelegate.From(OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var changedPropertyName = e.PropertyName;
            if (TypeInformationProvider.IsPropertyDataErrorInfoSource(Type, changedPropertyName) == false)
            {
                return;
            }
            var propertyValue = TypeInformationProvider.GetDataErrorInfoSourceValue(Type, changedPropertyName, this);
            if (propertyValue != null)
            {
                propertyValue.NotifyOn("Error", (o, o1) => NotifyOfPropertyChange(() => Error));
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                return GetErrorByPropertyName(columnName);
            }
        }

        private string GetErrorByPropertyName(string columnName)
        {
            var externalError = _externalErrors.ContainsKey(columnName) ? _externalErrors[columnName] : string.Empty;
            return string.Concat(externalError, GetInternalValidationErrorByPropertyName(columnName));
        }

        private string GetInternalValidationErrorByPropertyName(string columnName)
        {
            return ErrorService.GetValidationErrorByPropertyName(Type, columnName, this);
        }

        public virtual string Error
        {
            get
            {
                var ownError = CalculateOwnError();
                var childrenErrors = TypeInformationProvider.GetDataErrorInfoSourceValuesUnboxed(Type, this).Select(t => t.Error).ToArray();
                var stringBuilder = new StringBuilder();
                AppendErrorIfNeeded(ownError, stringBuilder);

                foreach (var childError in childrenErrors)
                {
                    AppendErrorIfNeeded(childError, stringBuilder);
                }
                return stringBuilder.ToString();
            }
        }

        private string CalculateOwnError()
        {
            var stringBuilder = new StringBuilder();
            foreach (var entry in TypeInformationProvider.GetValidationInfoCollection(Type))
            {
                var propError = GetErrorByPropertyName(entry.Key);
                stringBuilder.Append(propError);
            }
            return stringBuilder.ToString();
        }

        private static void AppendErrorIfNeeded(string ownError, StringBuilder stringBuilder)
        {
            if (string.IsNullOrWhiteSpace(ownError) == false)
            {
                stringBuilder.AppendLine(ownError);
            }
        }

        public bool HasErrors
        {
            get { return string.IsNullOrWhiteSpace(Error) == false; }
        }

        public void SetError(string error, string propertyName)
        {
            if (_externalErrors.ContainsKey(propertyName))
            {
                if (string.IsNullOrEmpty(error))
                {
                    _externalErrors.Remove(propertyName);
                }
                else
                {
                    _externalErrors[propertyName] = error;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(error) == false)
                {
                    _externalErrors.Add(propertyName, error);
                }
            }
            NotifyOfPropertyChange(() => HasErrors);
        }

        public void ClearError(string propertyName)
        {
            if (_externalErrors.ContainsKey(propertyName))
            {
                _externalErrors.Remove(propertyName);
            }
            NotifyOfPropertyChange(() => HasErrors);
        }
    }
}
