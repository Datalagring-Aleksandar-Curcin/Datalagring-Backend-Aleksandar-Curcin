using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.CourseRegistrations;

public class CreateCourseRegistrationDto
{
    [Required]
    public int CourseSessionId { get; set; }

    [Required]
    public int ParticipantId { get; set; }

    [Required]
    public int CourseStatusId { get; set; }

    public DateTime? RegistrationDate { get; set; }
}
