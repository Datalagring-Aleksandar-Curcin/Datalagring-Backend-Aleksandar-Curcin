namespace CoursesManager.Application.Dtos.Teachers;

public record TeacherDto(
    int Id,
    string FirstName,
    string LastName,
    string Expertise,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
