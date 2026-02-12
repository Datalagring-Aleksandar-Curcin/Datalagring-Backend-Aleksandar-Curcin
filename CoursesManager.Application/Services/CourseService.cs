using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class CourseService(ICourseRepository courseRepository)
{

    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task<ErrorOr<CourseDto>> CreateCourseAsync(CreateCourseDto dto, CancellationToken ct = default)
    {
        var exist = await _courseRepository.ExistAsync(x => x.CourseCode == dto.CourseCode);
        if (exist)
            return Error.Conflict("Course.Conflict", $"Course with '{dto.CourseCode}' already exists.");

        var savedCourse = await _courseRepository.CreateAsync(new CourseEntity { CourseCode = dto.CourseCode, Title = dto.Title, Description = dto.Description}, ct);
        return CourseMapper.ToCourseDto(savedCourse);
    }

    public async Task<ErrorOr<CourseDto>> GetOneCourseAsync(string courseCode, CancellationToken ct = default)
    {
        var course = await _courseRepository.GetOneAsync(x => x.CourseCode == courseCode, ct);
        return course is not null
            ? CourseMapper.ToCourseDto(course)
            : Error.NotFound("Courses.NotFound", $"Course with '{courseCode}' was not found.");
    }

    public async Task<IReadOnlyList<CourseDto>> GetAllCoursesAsync(CancellationToken ct = default)
    {
        return await _courseRepository.GetAllAsync(
            select: c => new CourseDto { CourseCode = c.CourseCode, Title = c.Title, Description = c.Description, CreatedAt = c.CreatedAt, RowVersion = c.RowVersion },
            orderBy: o => o.OrderByDescending(x => x.CreatedAt),
            ct: ct
            );
    }

    public async Task<ErrorOr<CourseDto>> UpdateCourseAsync(string courseCode, UpdateCourseDto dto, CancellationToken ct = default)
    {
        var course = await _courseRepository.GetOneAsync(x => x.CourseCode == courseCode, ct);
        if (course is null)
            return Error.NotFound("Courses.NotFound", $"Courses with '{courseCode}' was not found.");

        if (!course.RowVersion.SequenceEqual(dto.RowVersion))
            return Error.Conflict("Courses.Conflict", "Updated by another user. Try again.");

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.SaveChangesAsync(ct);
        return CourseMapper.ToCourseDto(course);
    }

    public async Task<ErrorOr<Deleted>> DeleteCourseAsync(string courseCode, CancellationToken ct = default)
    {
        var course = await _courseRepository.GetOneAsync(x => x.CourseCode == courseCode, ct);
        if (course is null)
            return Error.NotFound("Course.NotFound", $"Course with {courseCode} was not found.");

        await _courseRepository.DeleteAsync(course, ct);
        return Result.Deleted;
    }

}
