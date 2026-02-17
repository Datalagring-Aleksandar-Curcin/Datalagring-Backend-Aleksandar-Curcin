using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Persistence.Configurations;

public class CourseSessionConfiguration : IEntityTypeConfiguration<CourseSessionEntity>
{
    public void Configure(EntityTypeBuilder<CourseSessionEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.StartDate)
            .HasColumnType("datetime2(0)");

        builder.Property(e => e.EndDate)
            .HasColumnType("datetime2(0)");

        builder.ToTable("CourseSessions", t =>
        {
            t.HasCheckConstraint(
                $"CK_CourseSession_{nameof(CourseSessionEntity.MaxParticipants)}",
                $"[{nameof(CourseSessionEntity.MaxParticipants)}] > 0"
            );
        });

        builder.Property(e => e.CreatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(e => e.UpdatedAt)
            .HasColumnType("datetime2(0)")
            .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}
