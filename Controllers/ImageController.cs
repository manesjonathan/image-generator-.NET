using ImageGeneratorApi.Controllers.Requests;
using ImageGeneratorApi.Services;
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

        public ImageController(TokenService tokenService, AiService aiService, AuthService authService)
        {
            _tokenService = tokenService;
            _aiService = aiService;
            _authService = authService;
        }

        [HttpPost]
        [Route("generate")]
        public IActionResult GenerateImage([FromBody] ImageRequest request)
        {
            var validateToken = _tokenService.ValidateToken(request.Token);
            if (!validateToken)
            {
                return BadRequest("Invalid token");
            }

            var userEmail = _tokenService.GetUserFromToken(request.Token);

            var consumeBucket = _authService.ConsumeBucket(userEmail);
            if (!consumeBucket)
            {
                return BadRequest("No buckets left");
            }

            var generateImage = _aiService.CreateImage(request.Prompt);

            return Ok(generateImage);
        }
    }
}