namespace CoursesManager.Domain.Entities;

public class CourseRegistrationEntity
{
    public int Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsCanceled { get; set; }


    public int CourseSessionId { get; set; }
    public CourseSessionEntity CourseSession { get; set; } = null!;


    public int ParticipantId { get; set; }
    public ParticipantEntity Participant { get; set; } = null!;


    public int CourseStatusId { get; set; }
    public CourseStatusEntity CourseStatus { get; set; } = null!;
}
