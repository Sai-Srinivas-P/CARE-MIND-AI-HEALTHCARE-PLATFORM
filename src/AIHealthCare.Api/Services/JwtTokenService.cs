namespace AIHealthCare.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AIHealthCare.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAtUtc) Create(AppUser user);
}

public sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public (string Token, DateTime ExpiresAtUtc) Create(AppUser user)
    {
        var key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured.");

        if (key.Length < 32)
            throw new InvalidOperationException("Jwt:Key must be at least 32 characters.");

        var expires = DateTime.UtcNow.AddHours(2);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: credentials);
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
