using LibreCare.Data;
using LibreCare.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Services;

public class ProfileService
{
    private readonly LibreCareContext _context;

    public ProfileService(LibreCareContext context)
    {
        _context = context;
    }

    // For now all methods go here
    public async Task<List<Profile>> GetAllAsync()
    {
        return await _context.Profiles.ToListAsync();
    }

    public async Task<Profile?> GetByIdAsync(int id)
    {
        return await _context.Profiles.FindAsync(id);
    }

    public async Task<Profile?> GetActiveAsync()
    {
        return await _context.Profiles.FirstOrDefaultAsync(p => p.IsActive);
    }

    public async Task SetActiveAsync(int id)
    {
        // Deactivate others
        var profiles = await _context.Profiles.ToListAsync();
        foreach (var p in profiles) p.IsActive = false;

        // Activate selected
        var profile = await _context.Profiles.FindAsync(id);
        if (profile != null)
        {
            profile.IsActive = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateAsync(Profile profile)
    {
        var hasProfiles = await _context.Profiles.AnyAsync();

        // First profile → automatically active
        if (!hasProfiles) profile.IsActive = true;

        // If this one should be active → deactivate others
        if (profile.IsActive)
        {
            var profiles = await _context.Profiles.ToListAsync();
            foreach (var p in profiles) p.IsActive = false;
        }

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var profile = await _context.Profiles.FindAsync(id);
        if (profile == null) return;

        var wasActive = profile.IsActive;

        _context.Profiles.Remove(profile);
        await _context.SaveChangesAsync();

        if (wasActive)
        {
            var nextProfile = await _context.Profiles.FirstOrDefaultAsync();
            if (nextProfile != null)
            {
                nextProfile.IsActive = true;
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task UpdateAsync(Profile updatedProfile)
    {
        var existing = await _context.Profiles.FindAsync(updatedProfile.Id);
        if (existing == null) return;

        existing.Name = updatedProfile.Name;
        existing.BirthDate = updatedProfile.BirthDate;

        await _context.SaveChangesAsync();
    }
}
