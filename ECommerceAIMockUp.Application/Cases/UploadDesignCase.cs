using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.Wrappers;
using Microsoft.AspNetCore.Http;

namespace ECommerceAIMockUp.Application.Cases
{
    public class UploadDesignCase
    {
        private readonly IImageFileCreator _imageFileCreator;

        public UploadDesignCase(IImageFileCreator imageFileCreator)
        {
            _imageFileCreator = imageFileCreator;
        }
        private async Task<string> GetImageExtensionAsync(IFormFile uploadedImage)
        {
            byte[] buffer = new byte[4];

            try
            {
                using (var stream = uploadedImage.OpenReadStream())
                {
                    await stream.ReadAsync(buffer, 0, buffer.Length);
                }
            }
            catch
            {
                throw new Exception("Can not read uploaded image");
            }
            string[] validSignatures = new[]
            {
                "FFD8FF",    // JPEG & JPG
                "89504E47",  // PNG
            };
            string fileSignature = BitConverter.ToString(buffer).Replace("-","").ToUpperInvariant();
            if (fileSignature.StartsWith("FFD8FF"))
                return "JPEG";
            if (fileSignature.StartsWith("89504E47"))
                return "PNG";
            return "";
        }

        public async Task<Response<string>> SaveUploadedImage(IFormFile imageFile, string userId)
        {
            string extension = await GetImageExtensionAsync(imageFile);
            if (!string.IsNullOrEmpty(extension))
            {
                return new Response<string> { IsSucceeded = false, Error = "Unsupported format, supported format PNG, JPG, JPEG" };
            }
            string imagePath = await _imageFileCreator.CreateImageFileAsync(imageFile, extension);
            //save to database
            return new Response<string> { IsSucceeded = true, Data = "Uploaded" };
        }
    }
}
