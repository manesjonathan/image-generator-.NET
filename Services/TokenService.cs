using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ImageGeneratorApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace ImageGeneratorApi.Services
{
    public class TokenService
    {
        private const int ExpirationMinutes = 30;
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new JwtSecurityToken(
                issuer: "imageGeneratorApi",
                audience: "imageGeneratorApi",
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private List<Claim> CreateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };
            return claims;
        }

        private SigningCredentials CreateSigningCredentials()
        {
            var secret = _config.GetConnectionString("JWTSecret") ?? string.Empty;
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                SecurityAlgorithms.HmacSha256
            );
        }

        public bool ValidateToken(string token)
        {
            var secret = _config.GetConnectionString("JWTSecret") ?? string.Empty;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false, // Set to true if you want to validate the issuer
                    ValidateAudience = false, // Set to true if you want to validate the audience
                    ClockSkew = TimeSpan.Zero // Set to the desired clock skew
                };

                tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var expiration = jwtToken.ValidTo;

                return expiration > DateTime.UtcNow;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetUserFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Retrieve user claims from token
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email) ??
                             throw new Exception("No email claim found");

            return emailClaim.Value;
        }
    }
}