using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Application.Wrappers
{
    public class AllowedDesignPositionAttribute : ValidationAttribute
    {
        private readonly string[] _allowedValues;

        public AllowedDesignPositionAttribute(params string[] allowedValues)
        {
            _allowedValues = allowedValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !_allowedValues.Any(v => string.Equals(v, value.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                return new ValidationResult(ErrorMessage ?? $"Value must be one of: {string.Join(", ", _allowedValues)}");
            }

            return ValidationResult.Success;
        }
    }
}
