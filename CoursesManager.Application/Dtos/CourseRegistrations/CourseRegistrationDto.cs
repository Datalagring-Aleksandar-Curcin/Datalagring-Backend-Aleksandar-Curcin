namespace CoursesManager.Application.Dtos.CourseRegistrations;

public record CourseRegistrationDto(
    int Id,
    DateTime RegistrationDate,
    DateTime UpdatedAt,
    int CourseSessionId,
    int ParticipantId,
    int CourseStatusId,
    string StatusType
);
