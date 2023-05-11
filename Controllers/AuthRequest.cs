namespace TodoApi.Controllers;

public class AuthRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
}