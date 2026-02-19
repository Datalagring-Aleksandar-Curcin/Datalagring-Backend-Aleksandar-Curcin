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

        var entity = new CourseRegistrationEntity
        {
            CourseSessionId = dto.CourseSessionId,
            ParticipantId = dto.ParticipantId,
            CourseStatusId = dto.CourseStatusId,
            RegistrationDate = dto.RegistrationDate ?? DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var saved = await _repo.CreateAsync(entity, ct);
        return CourseRegistrationMapper.ToCourseRegistrationDto(saved);
    }

    public async Task<ErrorOr<CourseRegistrationDto>> GetOneCourseRegistrationAsync(int id, CancellationToken ct = default)
    {
        var reg = await _repo.GetOneAsync(
            where: r => r.Id == id,
            select: CourseRegistrationMapper.ToCourseRegistrationDtoExpr,
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
            ct: ct
        );
    }

    public async Task<ErrorOr<CourseRegistrationDto>> UpdateCourseRegistrationAsync(int id, UpdateCourseRegistrationDto dto, CancellationToken ct = default)
    {
        var reg = await _repo.GetOneAsync(r => r.Id == id, ct);
        if (reg is null)
            return Error.NotFound("CourseRegistration.NotFound", $"CourseRegistration with id '{id}' was not found.");

        reg.CourseStatusId = dto.CourseStatusId;
        reg.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync(ct);
        return CourseRegistrationMapper.ToCourseRegistrationDto(reg);
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
