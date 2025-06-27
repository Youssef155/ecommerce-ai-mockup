using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
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

        public async Task<Response<string>> ImageGenerator(string prompt)
        {
            var model = _configuration["HuggingFace:Model"];
            var requestUri = $"models/{model}";

            var payload = new
            {
                inputs = prompt,
                options = new { wait_for_model = true }
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Hugging Face API Error: {error}");
            }

            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            var rootPath = ProjectRootPathService.ProjectRootPath();
            var imagesDir = Path.Combine(rootPath, "images");
            Directory.CreateDirectory(imagesDir);

            var fileName = $"image_{Guid.NewGuid()}.png";
            var filePath = Path.Combine(imagesDir, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);

            return new Response<string>(filePath, HttpStatusCode.OK, true);

        }
    }
}
