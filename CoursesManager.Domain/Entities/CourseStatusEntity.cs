namespace CoursesManager.Domain.Entities;

public class CourseStatusEntity
{
    public int Id { get; set; }
    public string StatusType { get; set; } = null!;

    public ICollection<CourseRegistrationEntity> CourseRegistrations { get; set; } = [];
}
