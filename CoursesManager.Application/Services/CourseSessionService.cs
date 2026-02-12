using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Dtos.CourseSessions;
using CoursesManager.Application.Mappers;

namespace CoursesManager.Application.Services;

public class CourseSessionService(ICourseSessionRepository repo)
{

    
    public async Task<ErrorOr<CourseSessionDto>> GetOneCourseSessionAsync(int id, CancellationToken ct = default)
    {
        var courseSession = await repo.GetOneAsync(
            where: cs => cs.Id == id,
            select: CourseSessionMapper.ToCourseSessionDtoExpr,
            ct: ct
        );

        return courseSession is null
            ? Error.NotFound("CourseSession.NotFound", $"CourseSession with '{id}' was not found.")
            : courseSession;
    }
}



