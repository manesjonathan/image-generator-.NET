using ImageGeneratorApi.Controllers.Requests;
using ImageGeneratorApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageGeneratorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AiService _aiService;
        private readonly AuthService _authService;
        private readonly StorageService _storageService;

        public ImageController(TokenService tokenService, AiService aiService, AuthService authService,
            StorageService storageService)
        {
            _tokenService = tokenService;
            _aiService = aiService;
            _authService = authService;
            _storageService = storageService;
        }

        [HttpPost]
        [Route("generate")]
        [Authorize(AuthenticationSchemes =
            JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GenerateImage([FromBody] ImageRequest request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var userEmail = _tokenService.GetUserFromToken(token);

            var consumeBucket = _authService.ConsumeBucket(userEmail);
            if (!consumeBucket)
            {
                return BadRequest("No buckets left");
            }

            var generateImage = _aiService.CreateImage(request.Prompt);
            await _storageService.UploadFileAsync(generateImage);
            return Ok(generateImage);
        }

        [HttpGet]
        [Route("images")]
        [Authorize(AuthenticationSchemes =
            JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetImages()
        {
            var images = await _storageService.GetImagesUrlsAsync();
            return Ok(images);
        }
    }
}