using System.ComponentModel.DataAnnotations;

namespace ImageGeneratorApi.Controllers.Requests
{
    public class GoogleAuthRequest
    {
        public GoogleAuthRequest(string email, string name, string id)
        {
            Email = email;
            Name = name;
            Id = id;
        }

        [Required] public string Email { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Id { get; set; }
    }
}