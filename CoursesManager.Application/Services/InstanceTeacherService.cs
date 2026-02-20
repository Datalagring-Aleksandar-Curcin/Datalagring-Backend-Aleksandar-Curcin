using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.InstanceTeachers;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class InstanceTeacherService(IInstanceTeacherRepository repo)
{
    private readonly IInstanceTeacherRepository _repo = repo;

    public async Task<ErrorOr<InstanceTeacherDto>> AssignTeacherAsync(CreateInstanceTeacherDto dto, CancellationToken ct = default)
    {
        var exists = await _repo.ExistAsync(x => x.CourseSessionId == dto.CourseSessionId && x.TeacherId == dto.TeacherId);
        if (exists)
            return Error.Conflict("InstanceTeacher.Conflict", "Teacher is already assigned to this course session.");

        var saved = await _repo.CreateAsync(new InstanceTeacherEntity
        {
            CourseSessionId = dto.CourseSessionId,
            TeacherId = dto.TeacherId
        }, ct);

        return InstanceTeacherMapper.ToInstanceTeacherDto(saved);
    }

    public async Task<IReadOnlyList<InstanceTeacherDto>> GetAllAsync(CancellationToken ct = default)
    {
        return await _repo.GetAllAsync(
            select: InstanceTeacherMapper.ToInstanceTeacherDtoExpr,
            orderBy: q => q.OrderBy(x => x.CourseSessionId).ThenBy(x => x.TeacherId),
            ct: ct
        );
    }

    public async Task<IReadOnlyList<InstanceTeacherDto>> GetByCourseSessionAsync(int courseSessionId, CancellationToken ct = default)
    {
        return await repo.GetAllAsync(
            select: InstanceTeacherMapper.ToInstanceTeacherDtoExpr,
            where: x => x.CourseSessionId == courseSessionId,
            orderBy: q => q.OrderBy(x => x.TeacherId),
            ct: ct
        );
    }

    public async Task<ErrorOr<Deleted>> UnassignTeacherAsync(int courseSessionId, int teacherId, CancellationToken ct = default)
    {
        var row = await _repo.GetOneAsync(
            where: x => x.CourseSessionId == courseSessionId && x.TeacherId == teacherId,
            ct: ct
        );

        if (row is null)
            return Error.NotFound("InstanceTeacher.NotFound", "Assignment was not found.");

        await _repo.DeleteAsync(row, ct);
        return Result.Deleted;
    }
}