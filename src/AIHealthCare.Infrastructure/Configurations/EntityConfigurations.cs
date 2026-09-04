namespace AIHealthCare.Infrastructure.Configurations;
using AIHealthCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> b)
    {
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.Email).IsUnique();
        b.Property(x => x.Email).HasMaxLength(320).IsRequired();
        b.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
    }
}

public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> b)
    {
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.UserId).IsUnique();
        b.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        b.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        b.HasOne(x => x.User).WithOne(x => x.Patient).HasForeignKey<Patient>(x => x.UserId);
    }
}

public sealed class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> b)
    {
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.UserId).IsUnique();
        b.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        b.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        b.Property(x => x.Specialization).HasMaxLength(150).IsRequired();
        b.HasOne(x => x.User).WithOne(x => x.Doctor).HasForeignKey<Doctor>(x => x.UserId);
    }
}

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
        b.HasOne(x => x.Patient).WithMany(x => x.Appointments).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Doctor).WithMany(x => x.Appointments).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Notes).HasMaxLength(5000).IsRequired();
        b.Property(x => x.Diagnosis).HasMaxLength(2000);
        b.Property(x => x.FollowUpPlan).HasMaxLength(3000);
        b.HasOne(x => x.Patient).WithMany(x => x.MedicalRecords).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Doctor).WithMany(x => x.MedicalRecords).HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class MedicationConfiguration : IEntityTypeConfiguration<Medication>
{
    public void Configure(EntityTypeBuilder<Medication> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
        b.Property(x => x.Dosage).HasMaxLength(200).IsRequired();
        b.Property(x => x.Frequency).HasMaxLength(200).IsRequired();
        b.HasOne(x => x.Patient).WithMany(x => x.Medications).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class AiAuditLogConfiguration : IEntityTypeConfiguration<AiAuditLog>
{
    public void Configure(EntityTypeBuilder<AiAuditLog> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Feature).HasMaxLength(100).IsRequired();
        b.Property(x => x.RequestHash).HasMaxLength(128).IsRequired();
        b.Property(x => x.ResponseSummary).HasMaxLength(1000).IsRequired();
        b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
    }
}
