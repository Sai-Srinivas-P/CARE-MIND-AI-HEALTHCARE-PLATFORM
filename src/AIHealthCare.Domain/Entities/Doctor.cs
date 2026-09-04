namespace AIHealthCare.Domain.Entities;

public sealed class Doctor
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Specialization { get; set; }
    public string? LicenseNumber { get; set; }
    public AppUser User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = [];
}
