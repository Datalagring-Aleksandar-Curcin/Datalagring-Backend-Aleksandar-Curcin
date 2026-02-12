using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Dtos;
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

}
