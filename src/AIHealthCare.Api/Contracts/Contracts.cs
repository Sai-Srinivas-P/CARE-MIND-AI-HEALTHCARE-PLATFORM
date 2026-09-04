namespace AIHealthCare.Api.Contracts;

public sealed record LoginRequest(string Email, string Password);
public sealed record LoginResponse(string AccessToken, DateTime ExpiresAtUtc, string Role);

public sealed record CreatePatientRequest(
    string Email, string Password, string FirstName, string LastName, DateOnly DateOfBirth, string? Phone);

public sealed record PatientResponse(
    int Id, string FirstName, string LastName, DateOnly DateOfBirth, string? Phone);

public sealed record CreateAppointmentRequest(
    int PatientId, int DoctorId, DateTime ScheduledAtUtc, string Reason);

public sealed record AppointmentResponse(
    int Id, int PatientId, int DoctorId, DateTime ScheduledAtUtc, string Reason, string Status);

public sealed record AiQuestionRequest(string Question);
public sealed record AiAnswerResponse(string Answer);
public sealed record PatientSummaryResponse(int PatientId, string Summary);
