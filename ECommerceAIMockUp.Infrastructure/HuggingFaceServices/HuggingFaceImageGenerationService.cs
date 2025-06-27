using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ECommerceAIMockUp.Infrastructure.HuggingFaceServices
{
    public class HuggingFaceImageGenerationService : IImageGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public HuggingFaceImageGenerationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration; 
        }

        public async Task<Response<object>> ImageGenerator(string prompt)
        {
            var model = _configuration["HuggingFace:Model"];
            //var requestUri = $"{model}";

            var payload = new
            {
                response_format = "b64_json",
                prompt = prompt,
                model = "stability-ai/sdxl",
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("",content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Hugging Face API Error: {error}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<HuggingFaceImageResponse>(jsonString);

            string base64Image = result.Base64;
            string mimeType = result.MimeType;
            var extension = mimeType switch
            {
                "image/jpeg" => ".jpg",
                "image/webp" => ".webp",
                _ => ".png"
            };
            var imageBytes = Convert.FromBase64String(base64Image);
            var rootPath = ProjectRootPathService.ProjectRootPath();
            var imagesDir = Path.Combine(rootPath, "images");
            Directory.CreateDirectory(imagesDir);

            var fileName = $"image_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(imagesDir, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);

            return new Response<object>(result, HttpStatusCode.OK, true);

        }
    }
}
