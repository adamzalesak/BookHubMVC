using System.ComponentModel.DataAnnotations;

namespace WebMVC.Attributes;

public class PositiveNumber : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Null is not a positive number.");
        }

        var zero = Convert.ChangeType(0, value.GetType());

        if (value is IComparable comparableValue && comparableValue.CompareTo(zero) <= 0)
        {
            return new ValidationResult($"{validationContext.DisplayName} must be a positive number.");
        }

        return ValidationResult.Success;
    }
}