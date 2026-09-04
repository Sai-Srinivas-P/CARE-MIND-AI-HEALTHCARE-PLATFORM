namespace AIHealthCare.Api.Controllers;
using System.Security.Claims;
using AIHealthCare.Api.Contracts;
using AIHealthCare.Domain.Entities;
using AIHealthCare.Domain.Enums;
using AIHealthCare.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/patients")]
[Authorize]
public sealed class PatientsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await db.Patients
            .AsNoTracking()
            .OrderBy(x => x.LastName)
            .Select(x => new PatientResponse(x.Id, x.FirstName, x.LastName, x.DateOfBirth, x.Phone))
            .ToListAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PatientResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var patient = await db.Patients.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new { x.Id, x.UserId, x.FirstName, x.LastName, x.DateOfBirth, x.Phone })
            .SingleOrDefaultAsync(cancellationToken);

        if (patient is null) return NotFound();

        if (User.GetRole() == UserRole.Patient.ToString() && patient.UserId != User.GetUserId())
            return Forbid();

        return Ok(new PatientResponse(patient.Id, patient.FirstName, patient.LastName, patient.DateOfBirth, patient.Phone));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PatientResponse>> Create(CreatePatientRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName))
            return BadRequest(new { message = "Required fields are missing." });

        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(x => x.Email == email, cancellationToken))
            return Conflict(new { message = "Email already exists." });

        var patient = new Patient
        {
            User = new AppUser
            {
                Email = email,
                PasswordHash = DbSeeder.HashPassword(request.Password),
                Role = UserRole.Patient
            },
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            DateOfBirth = request.DateOfBirth,
            Phone = request.Phone?.Trim()
        };

        db.Patients.Add(patient);
        await db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = patient.Id },
            new PatientResponse(patient.Id, patient.FirstName, patient.LastName, patient.DateOfBirth, patient.Phone));
    }
}

internal static class ClaimsExtensions
{
    public static int? GetUserId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public static string? GetRole(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Role);
}
