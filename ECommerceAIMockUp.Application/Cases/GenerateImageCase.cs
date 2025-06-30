using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;

namespace ECommerceAIMockUp.Application.Cases
{
    public class GenerateImageCase
    {
        private readonly IPromptValidator _promptValidator;
        private readonly IImageGenerator _imageGenerator;
        private readonly IBaseRepository<AILog> _logRepository;
        public GenerateImageCase(IPromptValidator promptValidator, IImageGenerator imageGenerator, IBaseRepository<AILog> logRepository)
        {
            _promptValidator = promptValidator;
            _imageGenerator = imageGenerator;
            _logRepository = logRepository;
        }

        public async Task<Response<Image>> GenerateImageAsync(string prompt, string userId)
        {
            AILog aILog = new AILog { PromptText = prompt, AppUserId = userId };

            PromptValidationResult promptValidationResult = _promptValidator.ValidatePrompt(prompt);
            if (!promptValidationResult.IsValid)
            {
                aILog.IsSuccesed = false;
                await _logRepository.CreateAsync(aILog);
                await _logRepository.SaveChangesAsync();
                return new Response<Image> { IsSucceeded = false, Error = promptValidationResult.Error };
            }
            Image image = await _imageGenerator.GenerateImageAsync(prompt);
            aILog.IsSuccesed = true;
            //Add reference to the design in aiLog
            await _logRepository.CreateAsync(aILog);
            await _logRepository.SaveChangesAsync();
            return new Response<Image> { Data = image, IsSucceeded = true };
            
        }

    }
}
