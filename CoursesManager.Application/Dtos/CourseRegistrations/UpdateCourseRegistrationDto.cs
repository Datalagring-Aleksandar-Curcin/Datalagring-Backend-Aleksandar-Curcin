using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.CourseRegistrations;

public class UpdateCourseRegistrationDto
{
    [Required]
    public int CourseSessionId { get; set; }

    [Required]
    public int ParticipantId { get; set; }

    [Required]
    public int CourseStatusId { get; set; }
}
