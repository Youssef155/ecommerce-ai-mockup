using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Application.Contracts.ImageGenerators
{
    public interface IImageStorageService
    {
        Task<string> SaveAsync(byte[] imageBytes, string extension, params string[] folders);
        Task<string> SaveAsync(IFormFile image, string extension, params string[] folders);
        Task DeleteAsync(string fileName, params string[] folders);
    }
}
