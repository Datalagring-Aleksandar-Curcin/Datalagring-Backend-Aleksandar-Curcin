namespace CoursesManager.Domain.Entities;

public class TeacherEntity
{
    public int Id { get; set; }
    
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Expertise { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    

    public ICollection<InstanceTeacherEntity> InstanseTeacher { get; set; } = [];


}
