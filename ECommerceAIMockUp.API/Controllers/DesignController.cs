using System.Security.Claims;
using ECommerceAIMockUp.Application.Cases.DesignCases;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs.Design;
using ECommerceAIMockUp.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignController : ControllerBase
    {

        private readonly GenerateImageCase _generateImageCase;
        private readonly SaveImageCase _saveDesignCase;
        private readonly GetDesignsCase _getDesignCase;
        private readonly AddDesignDetailsCase _addDesignDetailsCase;

        public DesignController(GenerateImageCase generateImageCase, SaveImageCase saveDesignCase, GetDesignsCase getDesignCase, 
            AddDesignDetailsCase addDesignDetailsCase)
        {
            _generateImageCase = generateImageCase;
            _saveDesignCase = saveDesignCase;
            _getDesignCase = getDesignCase;
            _addDesignDetailsCase = addDesignDetailsCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetDesigns()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await _getDesignCase.GetDesignsAsync(userId);
            if (!response.IsSucceeded)
                return BadRequest(new { result = response.Error });
            return Ok(new { result = response.Data });
        }

        [HttpPost("generate")]
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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDesign(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest(new { Error = "Invalid file" });
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await _saveDesignCase.SaveUploadedImage(imageFile, userId);
            if (!response.IsSucceeded)
                return BadRequest(new { result = response.Error });
            return Ok(new { result = response.Data });
        }

        [HttpPost("save-generated")]
        public async Task<IActionResult> SaveGeneratedDesign(GeneratedDesign design)
        {
            if (design == null)
                return BadRequest(new { Error = "Invalid request body" });
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await _generateImageCase.SaveGeneratedImage(design, userId);
            if (!response.IsSucceeded)
                return BadRequest(new { result = response.Data });
            return Ok(new { result = response.Data });
        }

        [HttpPost("discard-generated")]
        public async Task<IActionResult> DiscardGeneratedDesign(GeneratedDesign design)
        {
            if (design == null)
                return BadRequest(new { Error = "Invalid request body" });
            var response = await _generateImageCase.DeleteGeneratedImageFile(design);
            if (!response.IsSucceeded)
                return BadRequest(new { result = response.Data });
            return Ok(new { result = response.Data });
        }

        [HttpPost("add-details")]
        public async Task<IActionResult> AddDesignDetails(DesignDetailsDTO designDetails)
        {
            var respone = await _addDesignDetailsCase.AddDesignDetailsAsync(designDetails);
            return Ok(new { result = respone.Data });
        }
    }
}
