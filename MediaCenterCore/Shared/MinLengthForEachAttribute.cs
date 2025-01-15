using System.ComponentModel.DataAnnotations;

namespace MediaCenterCore.Shared
{
    public class MinLengthForEachAttribute: ValidationAttribute
    {
        public MinLengthForEachAttribute(int length)
        {
            Length = length;
        }

        public int Length { get; }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IEnumerable<string> collection)
            {
                foreach (var item in collection)
                {
                    if (string.IsNullOrEmpty(item) || item.Length < Length)
                    {
                        return new ValidationResult(ErrorMessage ?? $"Each item must be at least {Length} characters long.");
                    }
                }
            }
            return ValidationResult.Success;
        }
       
    }
}
