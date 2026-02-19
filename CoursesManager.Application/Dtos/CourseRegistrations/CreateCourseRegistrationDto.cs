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

    // Valfritt: om du vill att klienten kan skicka datum.
    // Annars sätter vi RegistrationDate = UtcNow i service.
    public DateTime? RegistrationDate { get; set; }
}
