using Microsoft.Build.Framework;

namespace ImageGeneratorApi.Controllers;

public class GoogleAuthRequest
{
    public GoogleAuthRequest(string id, string email, string name)
    {
        Id = id;
        Email = email;
        Name = name;
    }

    [Required] public string Id { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Name { get; set; }
}