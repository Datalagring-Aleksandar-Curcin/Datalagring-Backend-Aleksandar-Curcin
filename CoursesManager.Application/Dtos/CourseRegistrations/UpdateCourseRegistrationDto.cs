using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.CourseRegistrations;

public class UpdateCourseRegistrationDto
{
    [Required]
    public int CourseStatusId { get; set; }
}
