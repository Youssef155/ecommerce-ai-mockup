using System.Security.Claims;
using ECommerceAIMockUp.Application.Cases;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs;
using ECommerceAIMockUp.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignController : ControllerBase
    {

        private readonly GenerateImageCase _generateImageCase;
        private readonly SaveDesignCase _saveDesignCase;

        public DesignController(GenerateImageCase generateImageCase, SaveDesignCase saveDesignCase)
        {
            _generateImageCase = generateImageCase;
            _saveDesignCase = saveDesignCase;
        }

        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateDesign(ImagePromptRequest request)
        {
            if (string.IsNullOrEmpty(request.Prompt))
                return BadRequest(new {Error = "Prompt is empty"});
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await _generateImageCase.GenerateImageAsync(request.Prompt, userId);
            if (!response.IsSucceeded)
                return BadRequest(response.Error);
            return Ok(response.Data);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadDesign(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest(new { Error = "Invalid file" });
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var respone = await _saveDesignCase.SaveUploadedImage(imageFile, userId);
            if (!respone.IsSucceeded)
                return BadRequest(respone.Error);
            return Ok(respone.Data);
        }
    }
}
