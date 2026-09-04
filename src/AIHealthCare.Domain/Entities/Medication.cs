namespace AIHealthCare.Domain.Entities;

public sealed class Medication
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public required string Name { get; set; }
    public required string Dosage { get; set; }
    public required string Frequency { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public Patient Patient { get; set; } = null!;
}
