using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Helper for calculating validation errors
    /// </summary>
    public static class ErrorService
    {
        /// <summary>
        /// Gets validation errors by property name
        /// </summary>
        /// <param name="type">Type of validated object</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="propertyContainer">Validated object</param>
        /// <returns>Textual representation of errors, if any; empty string otherwise</returns>
        public static string GetValidationErrorByPropertyName(Type type, string propertyName, object propertyContainer)
        {
            var validationInfo = TypeInformationProvider.GetValidationInfo(type, propertyName);
            if (validationInfo == null)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();            
            foreach (var validationAttribute in validationInfo.Item2)
            {
                var validationInfoValue = TypeInformationProvider.GetValidationInfoValue(type, propertyName,
                    propertyContainer);
                var validationResult = validationInfoValue == null
                    ? null
                    : validationAttribute.GetValidationResult(
                        validationInfoValue,
                        new ValidationContext(propertyName));
                if (validationResult != null)
                {
                    stringBuilder.Append(validationResult.ErrorMessage);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
