using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LogoFX.Client.Mvvm.Model
{
    public static class ErrorService
    {
        public static string GetValidationErrorByPropertyName(Type type,string propertyName, object propertyContainer)
        {
            var validationInfo = TypeInformationProvider.GetValidationInfo(type, propertyName);
            if (validationInfo == null)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();            
            foreach (var validationAttribute in validationInfo.Item2)
            {
                var validationResult =
                    validationAttribute.GetValidationResult(
                        TypeInformationProvider.GetValidationInfoValue(type, propertyName, propertyContainer),
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
