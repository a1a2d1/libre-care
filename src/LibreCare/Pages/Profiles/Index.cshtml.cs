using LibreCare.Data;
using LibreCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Pages.Profiles;

public class IndexModel : PageModel
{
    private readonly LibreCareContext _context;
    public IndexModel(LibreCareContext context)
    {
        _context = context;
    }

    public IList<Profile> Profiles { get; set; } = new List<Profile>();

    public async Task OnGetAsync()
    {
        Profiles = await _context.Profiles.ToListAsync();
    }

    public async Task<IActionResult> OnPostSetActiveAsync(int id)
    {
        var profiles = await _context.Profiles.ToListAsync();

        foreach (var p in profiles)
        {
            p.IsActive = p.Id == id;
        }

        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var profile = await _context.Profiles.FindAsync(id);

        if (profile == null)
        {
            return NotFound();
        }

        bool wasActive = profile.IsActive;

        _context.Profiles.Remove(profile);
        await _context.SaveChangesAsync();

        // If we deleted the active profile, set a new one
        if (!wasActive)
        {
            return RedirectToPage();
        }

        var nextProfile = await _context.Profiles.FirstOrDefaultAsync();
        if (nextProfile == null)
        {
            return RedirectToPage();
        }

        nextProfile.IsActive = true;
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
