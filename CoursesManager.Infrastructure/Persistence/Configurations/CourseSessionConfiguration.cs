using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoursesManager.Infrastructure.Persistence.Configurations;

public class CourseSessionConfiguration : IEntityTypeConfiguration<CourseSessionEntity>
{
    public void Configure(EntityTypeBuilder<CourseSessionEntity> builder)
    {
        builder.HasKey(e => e.Id);
    }
}
