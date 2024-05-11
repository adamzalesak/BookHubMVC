using System.ComponentModel.DataAnnotations;

namespace WebMVC.Attributes;

public class NonNegativeNumberAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Null is not a non-negative number.");
        }

        var zero = Convert.ChangeType(0, value.GetType());

        if (value is IComparable comparableValue && comparableValue.CompareTo(zero) < 0)
        {
            return new ValidationResult($"{validationContext.DisplayName} must be a non-negative number.");
        }

        return ValidationResult.Success;
    }
}
