using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Mappers;

public class CourseMapper
{

    public static CourseDto ToCourseDto(CourseEntity entity) => new()
    {
        CourseCode = entity.CourseCode,
        Title = entity.Title,
        Description = entity.Description,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt,
        RowVersion = entity.RowVersion
    };


}
