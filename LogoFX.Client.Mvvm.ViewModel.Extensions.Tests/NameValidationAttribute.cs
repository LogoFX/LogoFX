using System.ComponentModel.DataAnnotations;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    class NameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value as string;
            var isValid = str != null && str.Contains("$") == false;
            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Name is invalid");
            }
        }
    }
}