using ImageGeneratorApi.Data;
using ImageGeneratorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageGeneratorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ImageGeneratorApiContext _context;
    private readonly TokenService _tokenService;
    private readonly UserService _userService;

    public AuthController(ImageGeneratorApiContext context, TokenService tokenService, UserService userService)
    {
        _context = context;
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_userService.IsExistingUser(request.Email))
        {
            return BadRequest("User already exists");
        }

        var user = _userService.CreateUser(request.Email, request.Password);
        var accessToken = _tokenService.CreateToken(user);

        await _context.SaveChangesAsync();
        return Ok(new AuthResponse(user.Email, user.Name, accessToken));
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var verifiedUser = _userService.VerifyUser(request.Email, request.Password);
        var accessToken = _tokenService.CreateToken(verifiedUser);

        await _context.SaveChangesAsync();
        return Ok(new AuthResponse(verifiedUser.Email, verifiedUser.Name, accessToken));
    }

    [HttpPost]
    [Route("google-signin")]
    public async Task<ActionResult> GoogleSignIn([FromBody] GoogleAuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (_userService.IsExistingUser(request.Email))
        {
            var user = _userService.GetUserByEmailAndGoogleId(request.Email, request.Id);
            var token = _tokenService.CreateToken(user);
            return Ok(new AuthResponse(user.Email, user.Name, token));
        }

        var newUser = _userService.CreateGoogleUser(request.Email, request.Id, request.Name);
        var accessToken = _tokenService.CreateToken(newUser);
        await _context.SaveChangesAsync();
        return Ok(new AuthResponse(newUser.Email, newUser.Name, accessToken));
    }
}