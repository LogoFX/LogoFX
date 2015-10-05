using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{    
    partial class Model<T>
    {
        private readonly Dictionary<string, string> _externalErrors =
            new Dictionary<string, string>();

        private IErrorInfoExtractionStrategy _errorInfoExtractionStrategy;

        private void InitErrorListener()
        {
            ListenToPropertyChange();
            var interfaces = Type.GetInterfaces();
            //TODO: Add Chain-Of-Command
            if (interfaces.Contains(typeof(INotifyDataErrorInfo)))
            {
                _errorInfoExtractionStrategy = new NotifyDataErrorInfoExtractionStrategy();
                return;
            }
            if (interfaces.Contains(typeof(IDataErrorInfo)))
            {
                _errorInfoExtractionStrategy = new DataErrorInfoExtractionStrategy();
                return;
            }            
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
                NotifyOfPropertyChange(() => Error);
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
            if (string.IsNullOrEmpty(columnName))
            {
                foreach (var error in _externalErrors.Values)
                {
                    yield return error;
                }
            }
            else
            {
                var externalError = _externalErrors.ContainsKey(columnName) ? _externalErrors[columnName] : string.Empty;
                if (string.IsNullOrEmpty(externalError) == false)
                {
                    yield return externalError;
                }    
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
                var errors = GetAllErrors();
                return CreateErrorsPresentation(errors);                
            }
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

        public IEnumerable GetErrors(string propertyName)
        {
            return string.IsNullOrEmpty(propertyName) ? GetAllErrors() : GetErrorsByPropertyName(propertyName);
        }

        private IEnumerable<string> GetAllErrors()
        {
            var ownErrors = CalculateOwnErrors();
            var childrenErrors = _errorInfoExtractionStrategy.ExtractChildrenErrors(Type, this);
            var errors = ownErrors == null ? childrenErrors : ownErrors.Concat(childrenErrors);
            return errors;
        }

        public bool HasErrors
        {
            get { return string.IsNullOrWhiteSpace(Error) == false; }
        }

        protected void RaiseErrorsChanged([CallerMemberName] string name = "")
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(name));
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
            var errorsArray = errors == null ? null : errors.ToArray();
            if (errorsArray == null || errorsArray.Length == 0)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();
            foreach (var error in errorsArray)
            {
                AppendErrorIfNeeded(error, stringBuilder);
            }
            return stringBuilder.ToString();
        }

        private static void AppendErrorIfNeeded(string error, StringBuilder stringBuilder)
        {
            if (string.IsNullOrWhiteSpace(error) == false)
            {
                stringBuilder.AppendLine(error);
            }
        }
    }
}
