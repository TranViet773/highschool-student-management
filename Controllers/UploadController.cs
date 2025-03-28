using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ICloudinaryService cloudinaryService;
        public UploadController(ICloudinaryService cloudinaryService)
        {
            this.cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> uploadImage(IFormFile file)
        {
            var response = await cloudinaryService.uploadImageAsync(file);
            if(response.Error != null) return BadRequest(response.Error.Message);
            return Ok(response);
        } 
    }
}
