using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoursesManager.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _table;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }
    public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> findBy)
    {
        return await _table.AnyAsync(findBy);
    }


}