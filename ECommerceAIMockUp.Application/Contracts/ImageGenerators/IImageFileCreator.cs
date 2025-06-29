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
        Task<string> CreateImageFile(string base64Image, string extension);
        Task<string> CreateImageFile(IFormFile image, string extension);
    }
}
