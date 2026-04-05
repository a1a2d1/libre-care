using System.ComponentModel.DataAnnotations;

namespace LibreCare.Models;

public class Profile
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public required string Name { get; set; }

    public bool IsDefault  { get; set; }
    public DateOnly? BirthDate { get; set; } // Nullable

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
