using CoursesManager.Domain.Entities;
using CoursesManager.Infrastructure.Data;

namespace CoursesManager.Infrastructure.Repositories;

internal class CourseRepository(ApplicationDbContext context) : BaseRepository<CourseEntity>(context)
{




}
