using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Cases
{
    public class GenerateImageCase
    {
        private readonly IPromptValidator _promptValidator;
        private readonly IImageGenerator _imageGenerator;
        public GenerateImageCase(IPromptValidator promptValidator, IImageGenerator imageGenerator)
        {
            _promptValidator = promptValidator;
            _imageGenerator = imageGenerator;
        }

        public async Task<Response<ImageGenerationResult>> GenerateImageAsync(string prompt, string userId)
        {
            AILog aILog = new AILog { PromptText = prompt, AppUserId = userId };

            PromptValidationResult promptValidationResult = _promptValidator.ValidatePrompt(prompt);
            if (!promptValidationResult.IsValid)
            {
                aILog.IsSuccesed = false;
                //Save the aiLog to the database
                return new Response<ImageGenerationResult> { IsSucceeded = false, Error = promptValidationResult.Error };
            }
            ImageGenerationResult imageGenerationResult = await _imageGenerator.GenerateImageAsync(prompt);
            aILog.IsSuccesed = true;
            //Add reference to the design in aiLog
            //Save the aiLog to the database
            return new Response<ImageGenerationResult> { Data = imageGenerationResult, IsSucceeded = true };
            
        }

    }
}
