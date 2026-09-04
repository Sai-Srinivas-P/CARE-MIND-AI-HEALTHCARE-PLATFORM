namespace AIHealthCare.Domain.Entities;

public sealed class MedicalRecord
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime VisitDateUtc { get; set; }
    public required string Notes { get; set; }
    public string? Diagnosis { get; set; }
    public string? FollowUpPlan { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}
