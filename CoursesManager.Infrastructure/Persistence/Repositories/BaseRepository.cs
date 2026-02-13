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
    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        _table.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public virtual async Task<TEntity?> GetOneAsync(
       Expression<Func<TEntity, bool>> where,
       bool tracking = false,
       CancellationToken ct = default,
       params Expression<Func<TEntity, object>>[] includes)
    {
        return await BuildQuery(tracking, includes)
            .FirstOrDefaultAsync(where, ct);
    }

    public virtual async Task<TSelect?> GetOneAsync<TSelect>(
        Expression<Func<TEntity, bool>> where,
        Expression<Func<TEntity, TSelect>> select,
        bool tracking = false,
        CancellationToken ct = default)
    {
        return await _table
            .AsNoTracking()
            .Where(where)
            .Select(select)
            .FirstOrDefaultAsync(ct);
    }

    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> where, CancellationToken ct = default)
    {
        return await _table.FirstOrDefaultAsync(where, ct);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(
    Expression<Func<TEntity, bool>>? where = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    bool tracking = false,
    CancellationToken ct = default,
    params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(tracking, includes);

        if (where is not null)
            query = query.Where(where);

        if (orderBy is not null)
            query = orderBy(query);

        return await query.ToListAsync(ct);
    }
}