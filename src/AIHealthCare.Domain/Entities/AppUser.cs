namespace AIHealthCare.Domain.Entities;
using AIHealthCare.Domain.Enums;

public sealed class AppUser
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
}
