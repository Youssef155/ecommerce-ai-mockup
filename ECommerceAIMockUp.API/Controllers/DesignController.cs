using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignController : ControllerBase
    {

        private readonly IImageGenerator _imageGenerationService;

        public DesignController(IImageGenerator imageGenerationService)
        {
            _imageGenerationService = imageGenerationService;
        }
        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateDesign(ImagePromptRequest request)
        {
            if (string.IsNullOrEmpty(request.Prompt))
                return BadRequest(new {"Error" : "Prompt is empty"});
            var response = await _imageGenerationService.ImageGenerator(request.Prompt);
            return Ok(response);
        }
    }
}
