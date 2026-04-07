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
}
