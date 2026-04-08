using System.ComponentModel.DataAnnotations;

namespace LibreCare.Models;

public class Doctor
{
    [Key] public int Id { get; set; }

    [Required] public int ProfileId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 2)] public string? Specialty { get; set; }

    [Phone] [StringLength(20)] public string? Phone { get; set; }

    [StringLength(250)] public string? Address { get; set; }

    // UTC timestamp when the profile was created
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // UTC timestamp updated whenever the profile is modified
    // This won't update when the user makes a change
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
