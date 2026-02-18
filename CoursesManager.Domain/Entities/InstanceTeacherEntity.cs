namespace CoursesManager.Domain.Entities;

public class InstanceTeacherEntity
{
    public int CourseSessionId { get; set; }
    public CourseSessionEntity CourseSessions { get; set; } = null!;

    public int TeacherId { get; set; }
    public TeacherEntity Teachers { get; set; } = null!;
}
