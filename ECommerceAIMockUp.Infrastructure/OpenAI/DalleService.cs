using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ECommerceAIMockUp.Infrastructure.OpenAI
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

        public async Task<Response<object>> ImageGenerator(string prompt)
        {
            var response = await _openAI.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = prompt,
                N = _options.NumberOfImages,
                Size = _options.Size
            });
            if (!response.Successful)
            {
                return new Response<object>(response.Error?.Message, HttpStatusCode.BadRequest, false);
            }
            return new Response<object>(response.Results.Select(r => r.Url).FirstOrDefault(), HttpStatusCode.OK, true);
        }
    }
}
