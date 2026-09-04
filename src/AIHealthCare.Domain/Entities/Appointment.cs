namespace AIHealthCare.Domain.Entities;
using AIHealthCare.Domain.Enums;

public sealed class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime ScheduledAtUtc { get; set; }
    public required string Reason { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}
