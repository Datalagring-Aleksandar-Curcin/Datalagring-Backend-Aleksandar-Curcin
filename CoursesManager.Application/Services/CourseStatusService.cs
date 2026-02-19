using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.CourseStatus;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class CourseStatusService(ICourseStatusRepository repo)
{
    private readonly ICourseStatusRepository _repo = repo;

    public async Task<ErrorOr<CourseStatusDto>> CreateCourseStatusAsync(CreateCourseStatusDto dto, CancellationToken ct = default)
    {
        var exists = await _repo.ExistAsync(s => s.StatusType == dto.StatusType);
        if (exists)
            return Error.Conflict("CourseStatus.Conflict", $"Status '{dto.StatusType}' already exists.");

        var saved = await _repo.CreateAsync(new CourseStatusEntity
        {
            StatusType = dto.StatusType
        }, ct);

        return CourseStatusMapper.ToCourseStatusDto(saved);
    }

    public async Task<ErrorOr<CourseStatusDto>> GetOneCourseStatusAsync(int id, CancellationToken ct = default)
    {
        var status = await _repo.GetOneAsync(
            where: s => s.Id == id,
            select: CourseStatusMapper.ToCourseStatusDtoExpr,
            ct: ct
        );

        return status is null
            ? Error.NotFound("CourseStatus.NotFound", $"Status with id '{id}' was not found.")
            : status;
    }

    public async Task<IReadOnlyList<CourseStatusDto>> GetAllCourseStatusesAsync(CancellationToken ct = default)
    {
        return await _repo.GetAllAsync(
            select: CourseStatusMapper.ToCourseStatusDtoExpr,
            orderBy: q => q.OrderBy(s => s.StatusType),
            ct: ct
        );
    }

    public async Task<ErrorOr<CourseStatusDto>> UpdateCourseStatusAsync(int id, UpdateCourseStatusDto dto, CancellationToken ct = default)
    {
        var status = await _repo.GetOneAsync(s => s.Id == id, ct);
        if (status is null)
            return Error.NotFound("CourseStatus.NotFound", $"Status with id '{id}' was not found.");

        var statusTaken = await _repo.ExistAsync(s => s.StatusType == dto.StatusType && s.Id != id);
        if (statusTaken)
            return Error.Conflict("CourseStatus.Conflict", $"Status '{dto.StatusType}' already exists.");

        status.StatusType = dto.StatusType;

        await _repo.SaveChangesAsync(ct);
        return CourseStatusMapper.ToCourseStatusDto(status);
    }

    public async Task<ErrorOr<Deleted>> DeleteCourseStatusAsync(int id, CancellationToken ct = default)
    {
        var status = await _repo.GetOneAsync(s => s.Id == id, ct);
        if (status is null)
            return Error.NotFound("CourseStatus.NotFound", $"Status with id '{id}' was not found.");

        await _repo.DeleteAsync(status, ct);
        return Result.Deleted;
    }
}
