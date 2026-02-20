using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.InstanceTeachers;

public class CreateInstanceTeacherDto
{
    [Required]
    public int CourseSessionId { get; set; }

    [Required]
    public int TeacherId { get; set; }
}