using CoursesManager.Application.Dtos.CourseRegistrations;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public static class CourseRegistrationMapper
{
    public static Expression<Func<CourseRegistrationEntity, CourseRegistrationDto>> ToCourseRegistrationDtoExpr =>
        r => new CourseRegistrationDto(
            r.Id,
            r.RegistrationDate,
            r.UpdatedAt,
            r.CourseSessionId,
            r.ParticipantId,
            r.CourseStatusId
        );

    public static CourseRegistrationDto ToCourseRegistrationDto(CourseRegistrationEntity entity) =>
        new(
            entity.Id, 
            entity.RegistrationDate, 
            entity.UpdatedAt, 
            entity.CourseSessionId, 
            entity.ParticipantId, 
            entity.CourseStatusId
        );
}
