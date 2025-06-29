using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAIMockUp.Infrastructure.Services.ImageGeneration
{
    public static class ImageFileCreator
    {
        private static string GetProjectRootPath()
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

        private static string CreateImagesFolder()
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

        public static async Task<string> CreateImageFile(byte[] image, string extension)
        {
            string imagesFolderPath = CreateImagesFolder();
            string fileName = $"image_{Guid.NewGuid()}.{extension}";
            string filePath = Path.Combine(imagesFolderPath, fileName);
            try
            {
                await File.WriteAllBytesAsync(filePath, image);
                return filePath;
            }
            catch
            {
                throw new Exception("Can not write image file");
            }
        }
    }
}
