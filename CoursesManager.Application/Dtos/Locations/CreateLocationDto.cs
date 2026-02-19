using System.ComponentModel.DataAnnotations;

namespace CoursesManager.Application.Dtos.Locations;

public class CreateLocationDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
}
