using ECommerceAIMockUp.Application.Services.Interfaces.FileServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Infrastructure.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void DeleteFile(string fileName, string directory)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, directory, fileName);

            if (!Path.Exists(fullPath))
            {
                throw new FileNotFoundException($"File {fileName} does not exist");
            }
            File.Delete(fullPath);
        }

        public async Task<string> SaveFile(IFormFile file, string direction, string allowedExtentions)
        {
            var wwwPath = _webHostEnvironment.WebRootPath;
            var path = Path.Combine(wwwPath, direction);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var extensions = Path.GetExtension(file.FileName).ToLower();
            var allowedList = allowedExtentions.Split(",", StringSplitOptions.RemoveEmptyEntries)
                                                .Select(ext => ext.Trim().ToLower());

            if (!allowedList.Contains(extensions))
            {
                throw new InvalidOperationException($"Only {string.Join(", ", allowedList)} extensions are allowed");
            }


            var newFileName = $"{Guid.NewGuid()}{extensions}";

            var fullPath = Path.Combine(path, newFileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return newFileName;
        }

    }
}
