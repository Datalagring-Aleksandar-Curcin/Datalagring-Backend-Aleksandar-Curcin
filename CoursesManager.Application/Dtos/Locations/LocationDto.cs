namespace CoursesManager.Application.Dtos.Locations;

public record LocationDto(
    int Id,
    string Name,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
