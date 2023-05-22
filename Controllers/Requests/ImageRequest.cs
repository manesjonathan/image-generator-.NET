using System.ComponentModel.DataAnnotations;

namespace ImageGeneratorApi.Controllers.Requests
{
    public class ImageRequest
    {
        public ImageRequest(string prompt)
        {
            Prompt = prompt;
        }

        [Required] public string Prompt { get; set; }
    }
}