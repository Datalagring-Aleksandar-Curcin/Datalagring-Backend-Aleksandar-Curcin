using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Domain.Entities;
using CoursesManager.Infrastructure.Data;

namespace CoursesManager.Infrastructure.Persistence.Repositories
{
    public class InstanceTeacherRepository(ApplicationDbContext context) : BaseRepository<InstanceTeacherEntity>(context), IInstanceTeacherRepository
    {
    }
}
