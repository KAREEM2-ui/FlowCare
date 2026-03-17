using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FlowCare_presentation.validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FileSizeValidator : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FileSizeValidator class with the specified maximum file size in MB.
        /// </summary>
        /// <param name="FileSize">The maximum allowed file size, in Mb. Must be a positive integer.</param>
        /// 

        private readonly float _minSize;
        private readonly float _maxSize;
        public FileSizeValidator(float min,float max)
        {
            if (min < 0 || max < 0 || min > max)
                throw new ArgumentOutOfRangeException("Invalid range");

            this._minSize = min;
            this._maxSize = max;
        }



        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Allow null, use [Required] to enforce presence
            if (value is null)
            {
                return ValidationResult.Success;
            }



            IFormFile formFile = (value as IFormFile)!;

            if(formFile.Length > (_maxSize * 1024 * 1024) || formFile.Length < (_minSize * 1024 * 1024))
            {
                return new ValidationResult($"File size must be between {_minSize} MB and {_maxSize} MB.");
            }


            return ValidationResult.Success;
        }
    }
}
