

namespace CoursesManager.Domain.Entities;

public class CourseEntity
{
    public string CourseCode { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Byte[] RowVersion { get; set; } = null!;
}
