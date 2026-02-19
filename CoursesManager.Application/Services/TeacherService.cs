using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.Teachers;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class TeacherService(ITeacherRepository teacherRepository)
{
    private readonly ITeacherRepository _teacherRepository = teacherRepository;

    public async Task<ErrorOr<TeacherDto>> CreateTeacherAsync(CreateTeacherDto dto, CancellationToken ct = default)
    {

        var saved = await _teacherRepository.CreateAsync(new TeacherEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Expertise = dto.Expertise,
            UpdatedAt = DateTime.UtcNow
        }, ct);

        return TeacherMapper.ToTeacherDto(saved);
    }

    public async Task<ErrorOr<TeacherDto>> GetOneTeacherAsync(int id, CancellationToken ct = default)
    {
        var teacher = await _teacherRepository.GetOneAsync(
            where: t => t.Id == id,
            select: TeacherMapper.ToTeacherDtoExpr,
            ct: ct
        );

        return teacher is null
            ? Error.NotFound("Teacher.NotFound", $"Teacher with id '{id}' was not found.")
            : teacher;
    }

    public async Task<IReadOnlyList<TeacherDto>> GetAllTeachersAsync(CancellationToken ct = default)
    {
        return await _teacherRepository.GetAllAsync(
            select: TeacherMapper.ToTeacherDtoExpr,
            orderBy: q => q.OrderBy(t => t.LastName).ThenBy(t => t.FirstName),
            ct: ct
        );
    }

    public async Task<ErrorOr<TeacherDto>> UpdateTeacherAsync(int id, UpdateTeacherDto dto, CancellationToken ct = default)
    {
        var teacher = await _teacherRepository.GetOneAsync(t => t.Id == id, ct);
        if (teacher is null)
            return Error.NotFound("Teacher.NotFound", $"Teacher with id '{id}' was not found.");

        teacher.FirstName = dto.FirstName;
        teacher.LastName = dto.LastName;
        teacher.Expertise = dto.Expertise;
        teacher.UpdatedAt = DateTime.UtcNow;

        await _teacherRepository.SaveChangesAsync(ct);
        return TeacherMapper.ToTeacherDto(teacher);
    }

    public async Task<ErrorOr<Deleted>> DeleteTeacherAsync(int id, CancellationToken ct = default)
    {
        var teacher = await _teacherRepository.GetOneAsync(t => t.Id == id, ct);
        if (teacher is null)
            return Error.NotFound("Teacher.NotFound", $"Teacher with id '{id}' was not found.");

        await _teacherRepository.DeleteAsync(teacher, ct);
        return Result.Deleted;
    }
}
