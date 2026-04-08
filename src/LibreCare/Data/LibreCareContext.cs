using LibreCare.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Data;

public class LibreCareContext : DbContext
{
    public LibreCareContext(DbContextOptions<LibreCareContext> options)
        : base(options)
    {
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
}
