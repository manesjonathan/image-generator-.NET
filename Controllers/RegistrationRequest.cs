using Microsoft.Build.Framework;

namespace ImageGeneratorApi.Controllers;

public class RegistrationRequest
{
    public RegistrationRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}