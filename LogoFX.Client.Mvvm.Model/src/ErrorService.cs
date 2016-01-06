using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Helper for calculating validation errors
    /// </summary>
    public static class ErrorService
    {
        /// <summary>
        /// Gets validation errors description by property name
        /// </summary>
        /// <param name="type">Type of validated object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyContainer">Validated object</param>
        /// <returns>Textual representation of errors, if any; empty string otherwise</returns>
        public static string GetValidationErrorByPropertyName(Type type, string propertyName, object propertyContainer)
        {
            var validationErrors = GetValidationErrorsByPropertyNameInternal(type, propertyName, propertyContainer);
            if (validationErrors == null)
            {
                return null;
            }            
            var stringBuilder = new StringBuilder();            
            foreach (var validationError in validationErrors)
            {
                stringBuilder.Append(validationError);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets validation errors collection by property name
        /// </summary>
        /// <param name="type">Type of validated object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyContainer">Validated object</param>
        /// <returns>Collection of validation errors</returns>
        public static IEnumerable<string> GetValidationErrorsByPropertyName(Type type, string propertyName, object propertyContainer)
        {
            return GetValidationErrorsByPropertyNameInternal(type, propertyName, propertyContainer);
        }

        private static IEnumerable<string> GetValidationErrorsByPropertyNameInternal(Type type, string propertyName,
            object propertyContainer)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                var validationInfoCollection = TypeInformationProvider.GetValidationInfoCollection(type);
                foreach (var validationInfo in validationInfoCollection)
                {
                    var validationResults = GetValidationErrorsByPropertyNameInternal(type, validationInfo.Key,
                        propertyContainer);
                    if (validationResults != null)
                    {
                        foreach (var validationResult in validationResults)
                        {
                            yield return validationResult;
                        }
                    }
                }
            }
            else
            {
                var validationResults = GetValidationResultsInternal(type, propertyName, propertyContainer);
                foreach (var validationResult in validationResults)
                {
                    yield return validationResult;
                }
            }            
        }

        private static IEnumerable<string> GetValidationResultsInternal(Type type, string propertyName, object propertyContainer)
        {
            var validationInfo = TypeInformationProvider.GetValidationInfo(type, propertyName);
            if (validationInfo == null)
            {
                return null;
            }
            return GetValidationErrorsByPropertyNameFromValidationInfo(type, propertyName, propertyContainer,
                validationInfo);
        }

        private static IEnumerable<string> GetValidationErrorsByPropertyNameFromValidationInfo(
            Type type, string propertyName, object propertyContainer,
            Tuple<PropertyInfo, ValidationAttribute[]> validationInfo)
        {
            foreach (var validationAttribute in validationInfo.Item2)
            {
                var validationInfoValue = TypeInformationProvider.GetValidationInfoValue(type, propertyName,
                    propertyContainer);
                var validationResult = validationAttribute.GetValidationResult(
                        validationInfoValue,
                        new ValidationContext(propertyName));
                if (validationResult != null)
                {
                    yield return validationResult.ErrorMessage;
                }
            }   
        }
    }
}
