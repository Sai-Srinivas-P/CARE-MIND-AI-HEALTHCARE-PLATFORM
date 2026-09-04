namespace AIHealthCare.Api.Controllers;
using AIHealthCare.Api.Contracts;
using AIHealthCare.Api.Services;
using AIHealthCare.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(AppDbContext db, IJwtTokenService jwt) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null || !DbSeeder.VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid email or password." });

        var result = jwt.Create(user);
        return Ok(new LoginResponse(result.Token, result.ExpiresAtUtc, user.Role.ToString()));
    }
}
