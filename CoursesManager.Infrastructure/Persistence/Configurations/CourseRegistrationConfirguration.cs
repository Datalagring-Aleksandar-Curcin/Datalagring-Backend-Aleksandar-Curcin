using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Persistence.Configurations;

internal class CourseRegistrationConfirguration : IEntityTypeConfiguration<CourseRegistrationEntity>
{
    public void Configure(EntityTypeBuilder<CourseRegistrationEntity> builder)
    {
        builder.ToTable("CourseRegistrations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RegistrationDate)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("SYSUTCDATETIME()");


        builder.HasOne(x => x.Participant)
            .WithMany(p => p.CourseRegistrations)
            .HasForeignKey(x => x.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CourseSession)
            .WithMany(cs => cs.CourseRegistrations)
            .HasForeignKey(x => x.CourseSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CourseStatus)
            .WithMany(s => s.CourseRegistrations)
            .HasForeignKey(x => x.CourseStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
