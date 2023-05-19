using ImageGeneratorApi.Controllers.Requests;
using ImageGeneratorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageGeneratorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly AuthService _authService;

    public AuthController(TokenService tokenService, AuthService authService)
    {
        _tokenService = tokenService;
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public OkObjectResult Register([FromBody] RegistrationRequest request)
    {
        if (_authService.IsExistingUser(request.Email))
        {
            return new OkObjectResult(BadRequest("User already exists"));
        }

        var user = _authService.CreateUser(request.Email, request.Password);
        var accessToken = _tokenService.CreateToken(user);

        return Ok(new AuthResponse(user.Email, user.Name, accessToken));
    }

    [HttpPost]
    [Route("login")]
    public OkObjectResult Authenticate([FromBody] AuthRequest request)
    {
        var verifiedUser = _authService.VerifyUser(request.Email, request.Password);
        var accessToken = _tokenService.CreateToken(verifiedUser);

        return Ok(new AuthResponse(verifiedUser.Email, verifiedUser.Name, accessToken));
    }

    [HttpPost]
    [Route("google-signin")]
    public OkObjectResult GoogleSignIn([FromBody] GoogleAuthRequest request)
    {
        if (_authService.IsExistingUser(request.Email))
        {
            var user = _authService.GetUserByEmailAndGoogleId(request.Email,
                request.Id);
            var token = _tokenService.CreateToken(user);
            return Ok(new AuthResponse(user.Name, user.Email, token));
        }

        var newUser = _authService.CreateGoogleUser(request.Email, request.Id,
            request.Name);
        var accessToken = _tokenService.CreateToken(newUser);
        return Ok(new AuthResponse(newUser.Email, newUser.Name, accessToken));
    }
}