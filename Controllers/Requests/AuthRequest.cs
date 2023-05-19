using Microsoft.Build.Framework;

namespace ImageGeneratorApi.Controllers.Requests;

public class AuthRequest
{
    public AuthRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}