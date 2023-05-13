namespace ImageGeneratorApi.Controllers;

public class AuthResponse
{
    public AuthResponse(string username, string email, string token)
    {
        Username = username;
        Email = email;
        Token = token;
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}