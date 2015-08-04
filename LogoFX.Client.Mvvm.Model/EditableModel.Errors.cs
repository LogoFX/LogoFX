using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T>
    {
        private readonly Dictionary<string, string> _externalErrors =
            new Dictionary<string, string>();

        private readonly Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>> _withAttr =
            new Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>();

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
            if (_withAttr.ContainsKey(propertyName) == false)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();
            var propInfo = _withAttr[propertyName].Item1;
            foreach (var validationAttribute in _withAttr[propertyName].Item2)
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
                var stringBuilder = new StringBuilder();
                foreach (var tuple in _withAttr)
                {
                    var propError = GetErrorByPropertyName(tuple.Key);
                    stringBuilder.Append(propError);
                }
                return stringBuilder.ToString();
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
