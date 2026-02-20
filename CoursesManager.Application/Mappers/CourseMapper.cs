using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public static class CourseMapper
{
    public static Expression<Func<CourseEntity, CourseDto>> ToCourseDtoExpr =>
        c => new CourseDto(
            c.CourseId,
            c.CourseCode,
            c.Title,
            c.Description,
            c.CreatedAt,
            c.UpdatedAt,
            c.RowVersion
        );

    public static CourseDto ToCourseDto(CourseEntity c) =>
        new(
            c.CourseId,
            c.CourseCode,
            c.Title,
            c.Description,
            c.CreatedAt,
            c.UpdatedAt,
            c.RowVersion
        );
}