using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;
using CoursesManager.Infrastructure.Data;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class CourseStatusRepository(ApplicationDbContext context) : BaseRepository<CourseStatusEntity>(context), ICourseStatusRepository
{
}
