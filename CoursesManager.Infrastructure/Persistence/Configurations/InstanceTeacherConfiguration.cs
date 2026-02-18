using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Persistence.Configurations;

internal class InstanceTeacherConfiguration : IEntityTypeConfiguration<InstanceTeacherEntity>
{
    public void Configure(EntityTypeBuilder<InstanceTeacherEntity> builder)
    {
        builder.ToTable("InstanceTeachers");

        builder.HasKey(x => new { x.CourseSessionId, x.TeacherId });

        builder.HasOne(x => x.CourseSessions)
            .WithMany(cs => cs.InstanceTeachers)
            .HasForeignKey(x => x.CourseSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Teacher)
            .WithMany(t => t.InstanceTeachers)
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
