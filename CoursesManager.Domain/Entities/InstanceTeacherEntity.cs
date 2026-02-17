namespace CoursesManager.Domain.Entities;

public class InstanceTeacherEntity
{
    public int CourseSessionId { get; set; }
    public CourseSessionEntity CourseSession { get; set; } = null!;

    public int TeacherId { get; set; }
    public TeacherEntity Teacher { get; set; } = null!;
}
