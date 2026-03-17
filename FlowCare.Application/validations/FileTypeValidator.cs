using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace FlowCare_presentation.validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FileTypeValidatorAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        /// <summary>
        /// Accepts allowed extensions like ".jpg", ".png"
        /// </summary>
        /// <param name="allowedExtensions">Array of allowed extensions including dot (".jpg")</param>
        public FileTypeValidatorAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Allow null, use [Required] to enforce presence
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_allowedExtensions.Contains(extension))
                {
                    var allowed = string.Join(", ", _allowedExtensions);
                    return new ValidationResult($"Invalid file type. Allowed types: {allowed}");
                }
            }
            else
            {
                return new ValidationResult("Invalid file type.");
            }

            return ValidationResult.Success;
        }
    }
}