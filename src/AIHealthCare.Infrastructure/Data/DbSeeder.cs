namespace AIHealthCare.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;
using AIHealthCare.Domain.Entities;
using AIHealthCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, IConfiguration configuration)
    {
        await db.Database.MigrateAsync();
        if (await db.Users.AnyAsync()) return;

        var password = configuration["Seed:DemoPassword"] ?? "ChangeMe123!";

        var admin = new AppUser { Email = "admin@aihealthcare.local", PasswordHash = HashPassword(password), Role = UserRole.Admin };
        var doctorUser = new AppUser { Email = "doctor@aihealthcare.local", PasswordHash = HashPassword(password), Role = UserRole.Doctor };
        var patientUser = new AppUser { Email = "patient@aihealthcare.local", PasswordHash = HashPassword(password), Role = UserRole.Patient };

        db.Users.AddRange(admin, doctorUser, patientUser);
        await db.SaveChangesAsync();

        var doctor = new Doctor
        {
            UserId = doctorUser.Id,
            FirstName = "Asha",
            LastName = "Rao",
            Specialization = "General Medicine",
            LicenseNumber = "DEMO-001"
        };

        var patient = new Patient
        {
            UserId = patientUser.Id,
            FirstName = "Rahul",
            LastName = "Kumar",
            DateOfBirth = new DateOnly(1998, 4, 15),
            Phone = "+91-9000000000"
        };

        db.Doctors.Add(doctor);
        db.Patients.Add(patient);
        await db.SaveChangesAsync();

        db.Appointments.Add(new Appointment
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            ScheduledAtUtc = DateTime.UtcNow.AddDays(3),
            Reason = "Routine follow-up"
        });

        db.MedicalRecords.Add(new MedicalRecord
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            VisitDateUtc = DateTime.UtcNow.AddDays(-14),
            Notes = "Routine follow-up visit recorded for demonstration.",
            FollowUpPlan = "Discuss ongoing questions with the clinician."
        });

        db.Medications.Add(new Medication
        {
            PatientId = patient.Id,
            Name = "Demo Medication",
            Dosage = "As recorded by clinician",
            Frequency = "As prescribed",
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
        });

        await db.SaveChangesAsync();
    }

    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100_000, HashAlgorithmName.SHA256, 32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool VerifyPassword(string password, string stored)
    {
        var parts = stored.Split('.');
        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var expected = Convert.FromBase64String(parts[1]);
        var actual = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
