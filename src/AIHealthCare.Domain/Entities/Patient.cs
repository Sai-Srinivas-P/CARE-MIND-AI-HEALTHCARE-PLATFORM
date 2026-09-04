namespace AIHealthCare.Domain.Entities;

public sealed class Patient
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public AppUser User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = [];
    public ICollection<Medication> Medications { get; set; } = [];
}
