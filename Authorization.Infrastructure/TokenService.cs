using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authorization.Domain.DbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Infrastructure;

public class TokenService
{
    private readonly string _audience;
    private readonly string _issuer;
    private readonly SymmetricSecurityKey _key;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(IConfiguration configuration)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        _issuer = configuration["Jwt:Issuer"]!;
        _audience = configuration["Jwt:Audience"]!;
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
        _key = new SymmetricSecurityKey(key);
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public string GenerateJwtToken(User userAndRestaurant)
    {
        List<Claim> claims;


        claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userAndRestaurant.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("Name", $"{userAndRestaurant.FirstName} {userAndRestaurant.LastName}"),
            new("RoleId", userAndRestaurant.RoleId.ToString()),
            new("Email", userAndRestaurant.Email),
            new("PhoneNumber", userAndRestaurant.PhoneNumber ?? string.Empty)
        };

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: _signingCredentials);

        return _tokenHandler.WriteToken(token);
    }

    public static string GenerateRefreshToken() => Guid.NewGuid().ToString();

    public ClaimsPrincipal GetPrincipal(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = _key
        };

        var principal = _tokenHandler.ValidateToken(token, validationParameters, out _);

        return principal;
    }
}