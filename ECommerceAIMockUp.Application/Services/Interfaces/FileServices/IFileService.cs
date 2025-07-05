using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Application.Services.Interfaces.FileServices
{
    public interface IFileService
    {
        public Task<string> SaveFile(IFormFile file, string direction, string allowedExtentions);
        public void DeleteFile(string fileName, string directory);
    }
}
