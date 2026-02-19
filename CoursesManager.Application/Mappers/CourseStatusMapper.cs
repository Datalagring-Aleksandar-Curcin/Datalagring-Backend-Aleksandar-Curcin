using CoursesManager.Application.Dtos.CourseStatus;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public static class CourseStatusMapper
{
    public static Expression<Func<CourseStatusEntity, CourseStatusDto>> ToCourseStatusDtoExpr =>
        s => new CourseStatusDto(
            s.Id,
            s.StatusType
        );

    public static CourseStatusDto ToCourseStatusDto(CourseStatusEntity entity) =>
        new(entity.Id, entity.StatusType);
}
