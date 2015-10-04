using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    partial class Model<T> : IDataErrorInfo, IHaveErrors, IHaveExternalErrors
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
                var errors = GetErrorsByPropertyName(columnName);
                return CreateErrorsPresentation(errors);              
            }
        }

        private IEnumerable<string> GetErrorsByPropertyName(string columnName)
        {
            var externalError = _externalErrors.ContainsKey(columnName) ? _externalErrors[columnName] : string.Empty;
            if (string.IsNullOrEmpty(externalError) == false)
            {
                yield return externalError;
            }
            var validationErrors = GetInternalValidationErrorsByPropertyName(columnName);
            if (validationErrors != null)
            {
                foreach (var validationError in validationErrors)
                {
                    yield return validationError;
                }
            }            
        }

        private IEnumerable<string> GetInternalValidationErrorsByPropertyName(string columnName)
        {
            return ErrorService.GetValidationErrorsByPropertyName(Type, columnName, this);
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
            var ownErrors = CalculateOwnErrors();
            return CreateErrorsPresentation(ownErrors);
        }

        private IEnumerable<string> CalculateOwnErrors()
        {            
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var entry in TypeInformationProvider.GetValidationInfoCollection(Type))
            {
                var propErrors = GetErrorsByPropertyName(entry.Key);
                if (propErrors != null)
                {
                    foreach (var propError in propErrors)
                    {
                        yield return propError;
                    }
                }
            }            
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

        private static string CreateErrorsPresentation(IEnumerable<string> errors)
        {
            if (errors == null)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();
            foreach (var error in errors)
            {
                stringBuilder.Append(error);
            }
            return stringBuilder.ToString();
        }
    }
}
