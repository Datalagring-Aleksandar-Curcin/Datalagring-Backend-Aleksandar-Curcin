using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.CourseRegistrations;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class CourseRegistrationService(ICourseRegistrationRepository repo)
{
    private readonly ICourseRegistrationRepository _repo = repo;

    public async Task<ErrorOr<CourseRegistrationDto>> CreateCourseRegistrationAsync(CreateCourseRegistrationDto dto, CancellationToken ct = default)
    {
        var exists = await _repo.ExistAsync(r => r.ParticipantId == dto.ParticipantId && r.CourseSessionId == dto.CourseSessionId);
        if (exists)
            return Error.Conflict("CourseRegistration.Conflict", "Participant is already registered for this course session.");

        var saved = await _repo.CreateAsync(new CourseRegistrationEntity
        {
            ParticipantId = dto.ParticipantId,
            CourseSessionId = dto.CourseSessionId,
            CourseStatusId = dto.CourseStatusId,
            UpdatedAt = DateTime.UtcNow
            
        }, ct);

        return CourseRegistrationMapper.ToCourseRegistrationDto(saved);
    }

    public async Task<ErrorOr<CourseRegistrationDto>> GetOneCourseRegistrationAsync(int id, CancellationToken ct = default)
    {
        var reg = await _repo.GetOneAsync(
            where: r => r.Id == id,
            select: CourseRegistrationMapper.ToCourseRegistrationDtoExpr,
            includes: r => r.CourseStatus,
            ct: ct
        );

        return reg is null
            ? Error.NotFound("CourseRegistration.NotFound", $"CourseRegistration with id '{id}' was not found.")
            : reg;
    }

    public async Task<IReadOnlyList<CourseRegistrationDto>> GetAllCourseRegistrationsAsync(CancellationToken ct = default)
    {
        return await _repo.GetAllAsync(
            select: CourseRegistrationMapper.ToCourseRegistrationDtoExpr,
            orderBy: q => q.OrderByDescending(r => r.RegistrationDate),
            ct: ct,
            includes: r => r.CourseStatus
        );
    }

    public async Task<ErrorOr<CourseRegistrationDto>> UpdateCourseRegistrationAsync(int id, UpdateCourseRegistrationDto dto, CancellationToken ct = default)
    {
        var entity = await _repo.GetOneAsync(x => x.Id == id, ct);
        if (entity is null)
            return Error.NotFound("CourseRegistration.NotFound", $"CourseRegistration with id '{id}' was not found.");

        var duplicate = await _repo.ExistAsync(x =>
            x.Id != id &&
            x.ParticipantId == dto.ParticipantId &&
            x.CourseSessionId == dto.CourseSessionId);

        if (duplicate)
            return Error.Conflict(
                "CourseRegistration.Conflict",
                "Participant is already registered for this course session."
            );

        entity.ParticipantId = dto.ParticipantId;
        entity.CourseSessionId = dto.CourseSessionId;
        entity.CourseStatusId = dto.CourseStatusId;

        
        entity.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync(ct);
        return CourseRegistrationMapper.ToCourseRegistrationDto(entity);
    }


    public async Task<ErrorOr<Deleted>> DeleteCourseRegistrationAsync(int id, CancellationToken ct = default)
    {
        var reg = await _repo.GetOneAsync(r => r.Id == id, ct);
        if (reg is null)
            return Error.NotFound("CourseRegistration.NotFound", $"CourseRegistration with id '{id}' was not found.");

        await _repo.DeleteAsync(reg, ct);
        return Result.Deleted;
    }
}
