namespace AIHealthCare.Api.Controllers;
using AIHealthCare.Api.Contracts;
using AIHealthCare.Domain.Entities;
using AIHealthCare.Domain.Enums;
using AIHealthCare.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/appointments")]
[Authorize]
public sealed class AppointmentsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentResponse>>> Get(CancellationToken cancellationToken)
    {
        var query = db.Appointments.AsNoTracking();
        var role = User.GetRole();
        var userId = User.GetUserId();

        if (role == UserRole.Patient.ToString() && userId.HasValue)
            query = query.Where(x => x.Patient.UserId == userId.Value);
        else if (role == UserRole.Doctor.ToString() && userId.HasValue)
            query = query.Where(x => x.Doctor.UserId == userId.Value);

        return Ok(await query
            .OrderBy(x => x.ScheduledAtUtc)
            .Select(x => new AppointmentResponse(
                x.Id, x.PatientId, x.DoctorId, x.ScheduledAtUtc, x.Reason, x.Status.ToString()))
            .ToListAsync(cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<ActionResult<AppointmentResponse>> Create(
        CreateAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ScheduledAtUtc <= DateTime.UtcNow)
            return BadRequest(new { message = "Appointment must be in the future." });

        if (!await db.Patients.AnyAsync(x => x.Id == request.PatientId, cancellationToken) ||
            !await db.Doctors.AnyAsync(x => x.Id == request.DoctorId, cancellationToken))
            return BadRequest(new { message = "Patient or doctor does not exist." });

        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            ScheduledAtUtc = request.ScheduledAtUtc,
            Reason = request.Reason.Trim()
        };

        db.Appointments.Add(appointment);
        await db.SaveChangesAsync(cancellationToken);

        return Ok(new AppointmentResponse(
            appointment.Id, appointment.PatientId, appointment.DoctorId,
            appointment.ScheduledAtUtc, appointment.Reason, appointment.Status.ToString()));
    }

    [HttpPatch("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var appointment = await db.Appointments
            .Include(x => x.Patient).ThenInclude(x => x.User)
            .Include(x => x.Doctor).ThenInclude(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (appointment is null) return NotFound();

        var userId = User.GetUserId();
        var role = User.GetRole();

        var allowed = role == "Admin"
            || (role == "Patient" && appointment.Patient.UserId == userId)
            || (role == "Doctor" && appointment.Doctor.UserId == userId);

        if (!allowed) return Forbid();

        appointment.Status = AppointmentStatus.Cancelled;
        await db.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
