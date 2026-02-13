using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesManager.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<CourseSessionEntity> CourseSessions { get; set; }
    public DbSet<LocationEntity> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    }
}
