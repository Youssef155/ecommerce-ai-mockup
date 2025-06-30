using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Application.Contracts.ImageGenerators
{
    public interface IImageFileCreator
    {
        Task<string> CreateImageFileAsync(byte[] imageBytes, string extension);
        Task<string> CreateImageFileAsync(IFormFile image, string extension);
    }
}
