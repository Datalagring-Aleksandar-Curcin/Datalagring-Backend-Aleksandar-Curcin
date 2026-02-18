using CoursesManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesManager.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<CourseRegistrationEntity> CourseRegistrations { get; set; }
    public DbSet<CourseSessionEntity> CourseSessions { get; set; }
    public DbSet<CourseStatusEntity> CourseStatus { get; set; }
    public DbSet<InstanceTeacherEntity> InstanceTeachers { get; set; }
    public DbSet<LocationEntity> Locations { get; set; }
    public DbSet<ParticipantEntity> Participants { get; set; }
    public DbSet<TeacherEntity> Teachers { get; set; }





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    }
}
