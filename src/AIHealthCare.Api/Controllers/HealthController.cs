namespace AIHealthCare.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get() => Ok(new
    {
        status = "Healthy",
        service = "AI Healthcare Management API",
        utc = DateTime.UtcNow
    });
}
