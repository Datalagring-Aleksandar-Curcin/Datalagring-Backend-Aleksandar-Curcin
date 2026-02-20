using CoursesManager.Application.Dtos.InstanceTeachers;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public static class InstanceTeacherMapper
{
    public static Expression<Func<InstanceTeacherEntity, InstanceTeacherDto>> ToInstanceTeacherDtoExpr =>
        it => new InstanceTeacherDto(
            it.CourseSessionId,
            it.TeacherId
        );

    public static InstanceTeacherDto ToInstanceTeacherDto(InstanceTeacherEntity entity) =>
        new(
            entity.CourseSessionId, 
            entity.TeacherId
        );
}