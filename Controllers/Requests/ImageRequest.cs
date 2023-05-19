using System.ComponentModel.DataAnnotations;

namespace ImageGeneratorApi.Controllers.Requests
{
    public class ImageRequest
    {
        public ImageRequest(string prompt, string token)
        {
            Prompt = prompt;
            Token = token;
        }

        [Required] public string Prompt { get; set; }
        [Required] public string Token { get; set; }
    }
}