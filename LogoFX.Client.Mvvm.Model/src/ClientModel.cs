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
    /// <summary>
    /// Represents client model
    /// </summary>
    public abstract class ClientModel : NotifyPropertyChangedBase<ClientModel>, IClientModel
    {
        private readonly Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>> _withAttr =
            new Dictionary<string, Tuple<PropertyInfo, ValidationAttribute[]>>();

        /// <summary>
        /// Initializes a new instance of <see cref="ClientModel"/> class
        /// </summary>
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

        /// <summary>
        /// Gets the error with the specified column name.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
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

    /// <summary>
    /// Represents a value object
    /// </summary>
    public class ValueObject : ClientModel, IValueObject
    {
        
    }

    /// <summary>
    /// Represents entity ID
    /// </summary>
    /// <typeparam name="TEntityId">The type of the entity identifier.</typeparam>
    public class EntityId<TEntityId> : IEntityId<TEntityId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityId{TEntityId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public EntityId(TEntityId id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public TEntityId Id { get; set; }
    }
}
