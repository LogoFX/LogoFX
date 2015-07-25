using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using LogoFX.Client.Core;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    public abstract class ClientModel : NotifyPropertyChangedBase<ClientModel>, IClientModel
    {
        private readonly Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>> _withAttr =
            new Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>();

        protected ClientModel()
        {
            var props = GetType().GetProperties().ToArray();
            foreach (var propertyInfo in props)
            {
                var validationAttr = propertyInfo.GetCustomAttributes(typeof (ValidationAttribute), true).Cast<ValidationAttribute>().ToArray();
                if (validationAttr.Length > 0)
                {
                    _withAttr.Add(propertyInfo.Name,new Tuple<PropertyInfo, ValidationAttribute[]>(propertyInfo,validationAttr));
                }
            }            
        }

        public string this[string columnName]
        {
            get { return GetErrorByPropertyName(columnName); }
        }

        private string GetErrorByPropertyName(string propertyName)
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

        public string Error
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
    }

    public class ValueObject : ClientModel, IValueObject
    {
        
    }
}
