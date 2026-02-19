using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;
using CoursesManager.Infrastructure.Data;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class TeacherRepository(ApplicationDbContext context) : BaseRepository<TeacherEntity>(context), ITeacherRepository
{
}
