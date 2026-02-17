using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Persistence.Configurations;

public class CourseStatusConfiguration : IEntityTypeConfiguration<CourseStatusEntity>
{
    public void Configure(EntityTypeBuilder<CourseStatusEntity> builder)
    {
        builder.ToTable("CourseStatus");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StatusType)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.StatusType).IsUnique();
    }
}
