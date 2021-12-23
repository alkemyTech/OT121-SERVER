using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Interfaces.IServices.AWS;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class ImageTestController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageTestController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("image")]
        public async Task<IActionResult> ImageAsync(IFormFile file) 
        {
            var request = await _imageService.Save(file.FileName, file);

            return Ok(request);
        }
    }


}
