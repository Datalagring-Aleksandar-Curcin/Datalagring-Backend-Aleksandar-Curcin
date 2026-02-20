using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.Courses;

public class UpdateCourseDto
{
    [Required, MinLength(1), MaxLength(50)]
    public string Title { get; set; } = null!;

    [Required, MinLength(1), MaxLength(200)]
    public string Description { get; set; } = null!;

    [Required]
    public byte[] RowVersion { get; set; } = [];
}