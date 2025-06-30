using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Infrastructure.Services.ImageGeneration
{
    public class ImageFileCreator : IImageFileCreator
    {
        private string GetProjectRootPath()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            if (directory == null)
            {
                throw new Exception("Can not reach the project root path");
            }
            return directory?.FullName!;
        }

        private string CreateImagesFolder()
        {
            string rootPath = GetProjectRootPath();
            string imagesFolderPath = Path.Combine(rootPath, "Images");
            try
            {
                Directory.CreateDirectory(imagesFolderPath);
            }
            catch
            {
                throw new Exception("Can not create image file");
            }
            return imagesFolderPath;
        }

        public async Task<string> CreateImageFileAsync(byte[] imageBytes, string extension)
        {
            string imagesFolderPath = CreateImagesFolder();
            string fileName = $"image_{Guid.NewGuid()}.{extension}";
            string filePath = Path.Combine(imagesFolderPath, fileName);
            try
            {
                await File.WriteAllBytesAsync(filePath, imageBytes);
                return filePath;
            }
            catch
            {
                throw new Exception("Can not write image file");
            }
        }

        public async Task<string> CreateImageFileAsync(IFormFile image, string extension)
        {
            string imagesFolderPath = CreateImagesFolder();
            string fileName = $"image_{Guid.NewGuid()}.{extension}";
            string filePath = Path.Combine(imagesFolderPath, fileName);
            try
            {
                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(file);
                }
            }
            catch
            {
                throw new Exception("Can not write image file");
            }
            return filePath;
        }
    }
}
