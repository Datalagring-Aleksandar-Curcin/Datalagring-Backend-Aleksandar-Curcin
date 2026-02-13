using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;
using CoursesManager.Infrastructure.Data;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class CourseRepository(ApplicationDbContext context) : BaseRepository<CourseEntity>(context), ICourseRepository
{

}
