using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ECommerceAIMockUp.Infrastructure.Services.ImageGeneration.OpenAI
{
    public class DalleService : IImageGenerator
    {
        private readonly IOpenAIService _openAI;
        private readonly DalleImageOptions _options;

        public DalleService(IOptions<DalleImageOptions> options, IOpenAIService openAI)
        {
            _options = options.Value;
            _openAI = openAI;
        }

        public async Task<ImageGenerationResult> GenerateImageAsync(string prompt)
        {
            var response = await _openAI.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = prompt,
                N = _options.NumberOfImages,
                Size = _options.Size,
                ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Base64
            });
            if (!response.Successful || !response.Results.Any())
            {
                throw new Exception(response.Error?.Message ?? "DALL-E failed to generate image");
            }
            string base64Image = response.Results.First().B64;

            if (string.IsNullOrEmpty(base64Image))
            {
                throw new Exception("DALL-E response content is empty");
            }
            //string imageUrl = await ImageFileCreator.CreateImageFile(base64Image, "png");
            return new ImageGenerationResult { Base64Image = base64Image};
        }
    }
}
