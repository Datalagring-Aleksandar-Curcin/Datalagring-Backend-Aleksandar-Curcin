using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.CourseSessions;

public record UpdateCourseSessionDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int LocationId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int MaxParticipants { get; set; }
}
