using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.CourseStatus;

public class CreateCourseStatusDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string StatusType { get; set; } = null!;
}
