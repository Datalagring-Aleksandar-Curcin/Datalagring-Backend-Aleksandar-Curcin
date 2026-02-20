namespace CoursesManager.Application.Dtos.Courses;

public record CourseDto
(
    int CourseId,
    string CourseCode,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    byte[] RowVersion
);
