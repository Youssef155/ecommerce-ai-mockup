using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs.Design;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ECommerceAIMockUp.Application.Cases.DesignCases
{
    public class GenerateImageCase
    {
        private readonly IPromptValidator _promptValidator;
        private readonly IImageGenerator _imageGenerator;
        private readonly IBaseRepository<AILog> _logRepository;
        private readonly IImageStorageService _imageStorageService;
        private readonly IBaseRepository<Design> _designRepository;
        private readonly IConfiguration _config;

        public GenerateImageCase(IPromptValidator promptValidator, IImageGenerator imageGenerator, IBaseRepository<AILog> logRepository,
            IImageStorageService imageStorageService, IBaseRepository<Design> designRepository, IConfiguration config)
        {
            _promptValidator = promptValidator;
            _imageGenerator = imageGenerator;
            _logRepository = logRepository;
            _imageStorageService = imageStorageService;
            _designRepository = designRepository;
            _config = config;
        }

        private async Task AddDesignToDataBaseAsync(string userId, string imageName, int aiLogId)
        {
            try
            {
                Design design = new Design { AppUserId = userId, ImageUrl = imageName };
                design = await _designRepository.CreateAsync(design);
                await _designRepository.SaveChangesAsync();
                AILog aiLog = await _logRepository.GetByIdAsunc(aiLogId, true);
                if (aiLog != null)
                {
                    aiLog.DesignId = design.Id;
                }
                await _designRepository.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Can not add to database");
            }
        }

        public async Task<Response<GeneratedDesign>> GenerateImageAsync(string prompt, string userId)
        {
            AILog aiLog = new AILog { PromptText = prompt, AppUserId = userId };

            PromptValidationResult promptValidationResult = _promptValidator.ValidatePrompt(prompt);
            if (!promptValidationResult.IsValid)
            {
                aiLog.IsSuccesed = false;
                await _logRepository.CreateAsync(aiLog);
                await _logRepository.SaveChangesAsync();
                return new Response<GeneratedDesign> { IsSucceeded = false, Error = promptValidationResult.Error };
            }
            //byte[] imagebytes = await _imageGenerator.GenerateImageAsync(prompt);
            //if (imagebytes.Length == 0)
            //{
            //    aiLog.IsSuccesed = false;
            //    await _logRepository.CreateAsync(aiLog);
            //    await _logRepository.SaveChangesAsync();
            //    return new Response<GeneratedDesign> { IsSucceeded = false, Error = "Sorry could not generate image now, try again later" };
            //}
            //string imageName = await _imageStorageService.SaveAsync(imagebytes, "PNG", "designs");
            aiLog.IsSuccesed = true;
            await _logRepository.CreateAsync(aiLog);
            await _logRepository.SaveChangesAsync();
            string baseUrl = _config["ImageUrlSetting:BaseUrl"]!;
            //string imageUrl = $"{baseUrl.TrimEnd('/')}/designs/{imageName.TrimStart('/')}";
            string imageUrl = $"{baseUrl.TrimEnd('/')}/designs/image_31076f51-3f3e-44e7-8d04-a571eecb2e41 - Copy.png";
            GeneratedDesign generatedDesign = new GeneratedDesign() { ImageURL = imageUrl, PromptId = aiLog.Id};
            return new Response<GeneratedDesign> { Data = generatedDesign, IsSucceeded = true };
            
        }
        public async Task<Response<string>> SaveGeneratedImage(GeneratedDesign image, string userId)
        {
            string imageName = Uri.UnescapeDataString(new Uri(image.ImageURL).Segments.Last());
            await AddDesignToDataBaseAsync(userId, imageName, image.PromptId);
            return new Response<string> { IsSucceeded = true, Data = "Saved" };
        }

        public async Task<Response<string>> DeleteGeneratedImageFile(GeneratedDesign image)
        {
            string imageName = Uri.UnescapeDataString(new Uri(image.ImageURL).Segments.Last());
            await _imageStorageService.DeleteAsync(imageName, "images", "designs");
            return new Response<string> { IsSucceeded = true, Data = "Done" };
        }

    }
}
