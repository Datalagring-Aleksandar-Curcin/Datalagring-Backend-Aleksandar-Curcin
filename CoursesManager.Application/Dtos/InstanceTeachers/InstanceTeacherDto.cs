namespace CoursesManager.Application.Dtos.InstanceTeachers;

public record InstanceTeacherDto(
    int CourseSessionId,
    int TeacherId
);