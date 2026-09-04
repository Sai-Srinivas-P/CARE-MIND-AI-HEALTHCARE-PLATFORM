namespace AIHealthCare.Domain.Entities;

public sealed class AiAuditLog
{
    public long Id { get; set; }
    public int? UserId { get; set; }
    public required string Feature { get; set; }
    public required string RequestHash { get; set; }
    public required string ResponseSummary { get; set; }
    public long DurationMs { get; set; }
    public bool Success { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public AppUser? User { get; set; }
}
