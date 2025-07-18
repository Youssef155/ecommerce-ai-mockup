﻿using System;
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

        public async Task<byte[]> GenerateImageAsync(string prompt)
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
            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            return imageBytes;
        }
    }
}
