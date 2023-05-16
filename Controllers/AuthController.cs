using Google.Apis.Auth;
using ImageGeneratorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageGeneratorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly UserService _userService;

    public AuthController(TokenService tokenService, UserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost]
    [Route("register")]
    public OkObjectResult Register([FromBody] RegistrationRequest request)
    {
        if (_userService.IsExistingUser(request.Email))
        {
            return new OkObjectResult(BadRequest("User already exists"));
        }

        var user = _userService.CreateUser(request.Email, request.Password);
        var accessToken = _tokenService.CreateToken(user);

        return Ok(new AuthResponse(user.Email, user.Name, accessToken));
    }

    [HttpPost]
    [Route("login")]
    public OkObjectResult Authenticate([FromBody] AuthRequest request)
    {
        var verifiedUser = _userService.VerifyUser(request.Email, request.Password);
        var accessToken = _tokenService.CreateToken(verifiedUser);

        return Ok(new AuthResponse(verifiedUser.Email, verifiedUser.Name, accessToken));
    }

    [HttpPost]
    [Route("google-signin")]
    public OkObjectResult GoogleSignIn([FromBody] GoogleAuthRequest request)
    {
        if (_userService.IsExistingUser(request.Email))
        {
            var user = _userService.GetUserByEmailAndGoogleId(request.Email,
                request.Id);
            var token = _tokenService.CreateToken(user);
            return Ok(new AuthResponse(user.Name, user.Email, token));
        }

        var newUser = _userService.CreateGoogleUser(request.Email, request.Id,
            request.Name);
        var accessToken = _tokenService.CreateToken(newUser);
        return Ok(new AuthResponse(newUser.Email, newUser.Name, accessToken));
    }
}