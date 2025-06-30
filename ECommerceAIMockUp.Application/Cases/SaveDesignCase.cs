using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Application.Wrappers;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Application.Cases
{
    public class SaveDesignCase
    {
        private readonly IImageFileCreator _imageFileCreator;

        public SaveDesignCase(IImageFileCreator imageFileCreator)
        {
            _imageFileCreator = imageFileCreator;
        }
        private async Task<string> GetImageExtensionAsync(byte[] signatureBuffer)
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
        public async Task<Response<string>> SaveUploadedImage(IFormFile imageFile, string userId)
        {
            byte[] signatureBuffer = await GetSignatureBytes(imageFile);
            string extension = await GetImageExtensionAsync(signatureBuffer);
            if (!string.IsNullOrEmpty(extension))
            {
                return new Response<string> { IsSucceeded = false, Error = "Unsupported format, supported format PNG, JPG, JPEG" };
            }
            string imagePath = await _imageFileCreator.CreateImageFileAsync(imageFile, extension);
            //save to database
            return new Response<string> { IsSucceeded = true, Data = "Uploaded" };
        }

        public async Task<Response<string>> SaveGeneratedImage(Image image, string userId)
        {
            byte[] imageBytes = Convert.FromBase64String(image.Base64Image);
            if (imageBytes.Length == 0)
                return new Response<string> { IsSucceeded = false, Error = "No image data" };
            string extension = await GetImageExtensionAsync(imageBytes.Take(4).ToArray());
            string imagePath = await _imageFileCreator.CreateImageFileAsync(imageBytes, extension);
            //save to database
            return new Response<string> { IsSucceeded = true, Data = "Saved" };
        }
    }
}
