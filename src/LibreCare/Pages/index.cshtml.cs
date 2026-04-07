using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibreCare.Data;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Pages;

public class IndexModel : PageModel
{
    private readonly LibreCareContext _context;

    public IndexModel(LibreCareContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var profiles = await _context.Profiles.ToListAsync();

        if (!profiles.Any())
        {
            return RedirectToPage("/Profiles/Create");
        }

        var activeProfile = profiles.FirstOrDefault(p => p.IsActive);

        if (activeProfile == null)
        {
            activeProfile = profiles.First();
            activeProfile.IsActive = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("/Profiles/Dashboard", new { id = activeProfile.Id });
    }
}
