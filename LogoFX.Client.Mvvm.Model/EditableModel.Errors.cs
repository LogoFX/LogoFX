using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using LogoFX.Client.Mvvm.Core;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T>
    {
        private readonly Dictionary<string, string> _externalErrors =
            new Dictionary<string, string>();

        private readonly Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>> _withAttr =
            new Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>();

        private Dictionary<string, PropertyInfo> _dataErrorInfoProps;

        private void InitErrorListener()
        {
            var type = GetType();
            var props = type.GetProperties();
            _dataErrorInfoProps =
                props.Where(t => t.PropertyType.GetInterfaces().Contains(typeof(IDataErrorInfo)))
                    .ToDictionary(t => t.Name, t => t);
            ListenToPropertyChange();
        }

        private void ListenToPropertyChange()
        {
            PropertyChanged += WeakDelegate.From(OnPropertyChanged);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var changedPropertyName = e.PropertyName;
            if (_dataErrorInfoProps.ContainsKey(changedPropertyName) == false)
            {
                return;
            }
            var propertyValue = _dataErrorInfoProps[changedPropertyName].GetValue(this);
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
                var ownError = CalculateOwnError();
                var childrenErrors = _dataErrorInfoProps.Select(t => (t.Value.GetValue(this) as IDataErrorInfo).Error);
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
            foreach (var tuple in _withAttr)
            {
                var propError = GetErrorByPropertyName(tuple.Key);
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
