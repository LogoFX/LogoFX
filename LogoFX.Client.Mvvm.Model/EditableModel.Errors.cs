using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T>
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
            if (TypeInformationProvider.IsPropertyDataErrorInfoSource(_type, changedPropertyName) == false)
            {
                return;
            }
            var propertyValue = TypeInformationProvider.GetDataErrorInfoSourceValue(_type, changedPropertyName, this);
            if (propertyValue != null)
            {
                propertyValue.NotifyOn("Error", (o, o1) => NotifyOfPropertyChange(() => Error));
            }
        }

        public override string this[string columnName]
        {
            get
            {
                var externalError = _externalErrors.ContainsKey(columnName) ? _externalErrors[columnName] : string.Empty;
                return string.Concat(externalError, GetInternalValidationErrorByPropertyName(columnName));
            }
        }

        private string GetErrorByPropertyName(string columnName)
        {
            var externalError = _externalErrors.ContainsKey(columnName) ? _externalErrors[columnName] : string.Empty;
            return string.Concat(externalError, GetInternalValidationErrorByPropertyName(columnName));
        }

        private string GetInternalValidationErrorByPropertyName(string propertyName)
        {
            var validationInfo = TypeInformationProvider.GetValidationInfo(_type, propertyName);
            if (validationInfo == null)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();
            var propInfo = validationInfo.Item1;
            foreach (var validationAttribute in validationInfo.Item2)
            {
                var validationResult = validationAttribute.GetValidationResult(propInfo.GetValue(this), new ValidationContext(propertyName));
                if (validationResult != null)
                {
                    stringBuilder.Append(validationResult.ErrorMessage);
                }
            }
            return stringBuilder.ToString();
        }

        public override string Error
        {
            get 
            {               
                var ownError = CalculateOwnError();
                var childrenErrors = TypeInformationProvider.GetDataErrorInfoSourceValuesUnboxed(_type, this).Select(t => t.Error).ToArray();
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
            foreach (var entry in TypeInformationProvider.GetValidationInfoCollection(_type))
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

    //use this attribute to mark the properties
    //that should contibute to the Error property
    //of the containing object
    [AttributeUsage(AttributeTargets.Property)]
    class IncludeErrorAttribute : Attribute
    {

    }
}
