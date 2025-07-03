using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Infrastructure.Services.ImageGeneration
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }
        private Task<string> CreateDirectory(params string[] folders)
        {
            string webRootPath = _env.WebRootPath;
            string directoryPath = Path.Combine(webRootPath, "images");
            foreach (var folder in folders)
            {
                directoryPath = Path.Combine(directoryPath, folder);
            }
            if (!Directory.Exists(directoryPath))
            {
                try
                {
                    Directory.CreateDirectory(directoryPath);
                }
                catch
                {
                    throw new Exception($"can not create {directoryPath} directory");
                }
            }
            return Task.FromResult(directoryPath);
        }

        public async Task<string> SaveAsync(byte[] imageBytes, string extension, params string[] folders)
        {
            string imagesFolderPath = await CreateDirectory(folders);
            string fileName = $"image_{Guid.NewGuid()}.{extension}";
            string filePath = Path.Combine(imagesFolderPath, fileName);
            try
            {
                await File.WriteAllBytesAsync(filePath, imageBytes);
                return fileName;
            }
            catch
            {
                throw new Exception("Can not write image file");
            }
        }

        public async Task<string> SaveAsync(IFormFile image, string extension, params string[] folders)
        {
            string imagesFolderPath = await CreateDirectory(folders);
            string fileName = $"image_{Guid.NewGuid()}.{extension}";
            string filePath = Path.Combine(imagesFolderPath, fileName);
            try
            {
                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(file);
                }
                return fileName;
            }
            catch
            {
                throw new Exception("Can not write image file");
            }
        }

        public Task DeleteAsync(string fileName, params string[] folders)
        {
            string filePath = Path.Combine(_env.WebRootPath, fileName);
            foreach (var folder in folders)
            {
                filePath = Path.Combine(filePath, folder);
            }
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    throw new Exception($"can not delete {fileName}");
                }
            }
            return Task.CompletedTask;
        }
    }
}
