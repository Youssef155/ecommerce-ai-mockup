using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceAIMockUp.Infrastructure.Services.ImageGeneration
{
    public class PromptValidator : IPromptValidator
    {
        private readonly int _maxLength;
        private readonly int _minLength;
        private readonly HashSet<string> _blookedTerms;

        public PromptValidator(IConfiguration config)
        {
            _maxLength = config.GetValue<int>("PromptSettings:MaxLength");
            _minLength = config.GetValue<int>("PromptSettings:MinLength");
            _blookedTerms = new HashSet<string>(
            config.GetSection("PromptSettings:BlockedTerms").Get<string[]>() ?? Array.Empty<string>(),
            StringComparer.OrdinalIgnoreCase);

        }
        public PromptValidationResult ValidatePrompt(string prompt)
        {
            prompt = Regex.Replace(prompt.Trim(), @"\s+", " ");
            if (Regex.Replace(prompt, " ", "").Length < _minLength)
                return new PromptValidationResult{ IsValid = false, Error = $"Prompt must be more that {_minLength} characters" };
     
            if (prompt.Length > _maxLength)
                return new PromptValidationResult{ IsValid = false, Error = $"Prompt must be less than {_maxLength} characters" };

            foreach (var term in _blookedTerms)
            {
                if (prompt.Contains(term))
                    return new PromptValidationResult{ IsValid = false, Error = $"Prompt can not contain word {term}" };

            }
            return new PromptValidationResult { IsValid = true };
        }

    }
}
