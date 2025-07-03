using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Application.Wrappers;
using ECommerceAIMockUp.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Application.Cases
{
    public class SaveImageCase
    {
        private readonly IImageStorageService _imageStorageService;
        private readonly IBaseRepository<Design> _designRepository;

        public SaveImageCase(IImageStorageService imageStorageService, IBaseRepository<Design> designRepository)
        {
            _imageStorageService = imageStorageService;
            _designRepository = designRepository;
        }
        private string GetImageExtensionAsync(byte[] signatureBuffer)
        {
            
            string fileSignature = BitConverter.ToString(signatureBuffer).Replace("-","").ToUpperInvariant();
            if (fileSignature.StartsWith("FFD8FF"))
                return "JPEG";
            if (fileSignature.StartsWith("89504E47"))
                return "PNG";
            return "";
        }

        private async Task<byte[]> GetSignatureBytes(IFormFile imageFile)
        {
            byte[] buffer = new byte[4];

            try
            {
                using (var stream = imageFile.OpenReadStream())
                {
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                throw new Exception("Can not read uploaded image");
            }
            return buffer;
        }

        private async Task AddImageToDataBaseAsync(string userId, string imagePath)
        {
            try
            {
                Design design = new Design { AppUserId = userId, ImageUrl = imagePath };
                await _designRepository.CreateAsync(design);
                await _designRepository.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Can not add to database");
            }
        }
        public async Task<Response<string>> SaveUploadedImage(IFormFile imageFile, string userId)
        {
            byte[] signatureBuffer = await GetSignatureBytes(imageFile);
            string extension = GetImageExtensionAsync(signatureBuffer);
            if (!string.IsNullOrEmpty(extension))
            {
                return new Response<string> { IsSucceeded = false, Error = "Unsupported format, supported format PNG, JPG, JPEG" };
            }
            string imagePath = await _imageStorageService.SaveAsync(imageFile, extension, "designs");
            await AddImageToDataBaseAsync(userId, imagePath);
            return new Response<string> { IsSucceeded = true, Data = "Uploaded" };
        }

    }
}
