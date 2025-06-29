using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Application.Wrappers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ECommerceAIMockUp.Infrastructure.Services.ImageGeneration.StabiliytAIServices
{
    public class StabilityAIImageGenerationService : IImageGenerator
    {
        private readonly HttpClient _httpClient;

        public StabilityAIImageGenerationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ImageGenerationResult> GenerateImageAsync(string prompt)
        {
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(prompt), "\"prompt\"");
            form.Add(new StringContent("png"), "\"output_format\"");

            var response = await _httpClient.PostAsync("",form);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Stability AI API Error: {error}");
            }
            var base64Image = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(base64Image))
            {
                throw new Exception("Stability AI respone is empty");
            }
            //string imageUrl = await ImageFileCreator.CreateImageFile(base64Image, "png");
            return new ImageGenerationResult { Base64Image = base64Image};
        }
    }
}
