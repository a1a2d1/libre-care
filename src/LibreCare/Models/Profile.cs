using System.ComponentModel.DataAnnotations;

namespace LibreCare.Models;

public class Profile
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100, MinimumLength = 2)]
    public required string Name { get; set; }

    // Indicates whether this is the default profile for the user
    public bool IsActive  { get; set; }
    public DateOnly? BirthDate { get; set; } // Nullable

    // UTC timestamp when the profile was created
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // UTC timestamp updated whenever the profile is modified
    // This won't update when the user makes a change
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
