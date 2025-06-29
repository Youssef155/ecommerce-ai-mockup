using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.DTOs;

namespace ECommerceAIMockUp.Application.Contracts.ImageGenerators
{
    public interface IPromptValidator
    {
        PromptValidationResult ValidatePrompt(string promptText);
    }
}
