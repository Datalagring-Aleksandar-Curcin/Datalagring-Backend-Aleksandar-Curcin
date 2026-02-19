using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.CourseSessions;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class CourseSessionService(ICourseSessionRepository repo)
{
    private readonly ICourseSessionRepository _repo = repo;

    public async Task<ErrorOr<CourseSessionDto>> CreateCourseSessionAsync(CreateCourseSessionDto dto, CancellationToken ct = default)
    {
        if (dto.EndDate <= dto.StartDate)
            return Error.Validation("CourseSession.InvalidDates", "EndDate must be after StartDate.");

        var saved = await _repo.CreateAsync(new CourseSessionEntity
        {
            CourseId = dto.CourseId,
            LocationId = dto.LocationId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            MaxParticipants = dto.MaxParticipants,
            UpdatedAt = DateTime.UtcNow
        }, ct);

        var created = await _repo.GetOneAsync(
            where: cs => cs.Id == saved.Id,
            select: CourseSessionMapper.ToCourseSessionDtoExpr,
            ct: ct
        );

        return created!;
    }

    public async Task<ErrorOr<CourseSessionDto>> GetOneCourseSessionAsync(int id, CancellationToken ct = default)
    {
        var courseSession = await _repo.GetOneAsync(
            where: cs => cs.Id == id,
            select: CourseSessionMapper.ToCourseSessionDtoExpr,
            ct: ct
        );

        return courseSession is null
            ? Error.NotFound("CourseSession.NotFound", $"CourseSession with '{id}' was not found.")
            : courseSession;
    }

    public async Task<IReadOnlyList<CourseSessionDto>> GetAllCourseSessionsAsync(CancellationToken ct = default)
    {
        return await _repo.GetAllAsync(
            select: CourseSessionMapper.ToCourseSessionDtoExpr,
            orderBy: q => q.OrderByDescending(x => x.CreatedAt),
            ct: ct
        );
    }

    public async Task<ErrorOr<CourseSessionDto>> UpdateCourseSessionAsync(int id, UpdateCourseSessionDto dto, CancellationToken ct = default)
    {
        if (dto.EndDate <= dto.StartDate)
            return Error.Validation("CourseSession.InvalidDates", "EndDate must be after StartDate.");

        var entity = await _repo.GetOneAsync(cs => cs.Id == id, ct);
        if (entity is null)
            return Error.NotFound("CourseSession.NotFound", $"CourseSession with '{id}' was not found.");

        entity.CourseId = dto.CourseId;
        entity.LocationId = dto.LocationId;
        entity.StartDate = dto.StartDate;
        entity.EndDate = dto.EndDate;
        entity.MaxParticipants = dto.MaxParticipants;
        entity.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync(ct);

        var updated = await _repo.GetOneAsync(
            where: cs => cs.Id == id,
            select: CourseSessionMapper.ToCourseSessionDtoExpr,
            ct: ct
        );

        return updated!;
    }

    public async Task<ErrorOr<Deleted>> DeleteCourseSessionAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetOneAsync(cs => cs.Id == id, ct);
        if (entity is null)
            return Error.NotFound("CourseSession.NotFound", $"CourseSession with '{id}' was not found.");

        await _repo.DeleteAsync(entity, ct);
        return Result.Deleted;
    }
}
