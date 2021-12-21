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
        public async Task<IActionResult> ImageAsync(IFormFile file) // [FromForm] 
        {
            var request = await _imageService.SaveImageAsync(file);

            return Ok(request);
        }

        [HttpGet("getimage")]
        public ActionResult GetUrl(string namefile)
        {
            var request = _imageService.GetImageUrl(namefile);

            return Ok(request);
        }
    }


}
